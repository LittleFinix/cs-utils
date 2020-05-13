using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace Finix.CsUtils
{
    public static class ProcessUtil
    {
        public static Task<int> InvokeAsync(string prog,
                                     IEnumerable<string> more_args,
                                     Stream? stdin = null,
                                     Stream? stdout = null,
                                     Stream? stderr = null)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = prog;

            foreach (var arg in more_args)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            process.EnableRaisingEvents = true;

            void onExit()
            {
                if (!process.HasExited)
                    process.WaitForExit();

                tcs.SetResult(process.ExitCode);

                process.Close();
            }

            var args = process.StartInfo.ArgumentList.Select(arg => EscapeArgument(arg, true));
            Trace.WriteLine($"Invoking: {process.StartInfo.FileName} {string.Join(" ", args)}");

            StartAndRedirectOutputs(process, onExit, stdin, stdout, stderr);

            return tcs.Task;
        }

        public static string EscapeArgument(string arg, bool quoted = false)
        {
            const char CHAR_TO_ESCAPE = '"';

            arg = Regex.Replace(arg, @"(\\*)" + CHAR_TO_ESCAPE, @"$1$1\" + CHAR_TO_ESCAPE);

            return !quoted
                ? arg
                : CHAR_TO_ESCAPE + Regex.Replace(arg, @"(\\+)$", @"$1$1") + CHAR_TO_ESCAPE;
        }

        public static void AddArgument(ICollection<string> args, string param, object? param_arg = null, bool escapeParam = true)
        {
            args.Add(escapeParam ? EscapeArgument(param) : param);

            if (param_arg?.ToString() is string s)
                args.Add(EscapeArgument(s));
        }

        public static void StartAndRedirectOutputs(System.Diagnostics.Process process, Action onExit, Stream? stdin, Stream? stdout, Stream? stderr)
        {
            // Always redirect stdout and stderr to make sure it's output doesn't show up in the console
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = stdout != null;
            process.StartInfo.RedirectStandardError = stderr != null;

            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;

            // var copyTasks = new Barrier(4);
            // var copyTasks = Task.CompletedTask;
            var copyTasks = new SemaphoreSlim(3);

            process.Exited += async (sender, args) => {

                if (!process.HasExited)
                    process.WaitForExit();

                await copyTasks.WaitAsync().ConfigureAwait(false);
                // await Task.WhenAll(stdout?.FlushAsync() ?? Task.CompletedTask,
                //                    stderr?.FlushAsync() ?? Task.CompletedTask);

                // process.StandardInput.Close();
                // process.StandardOutput.Close();
                // process.StandardError.Close();

                onExit();
            };

            process.Start();

            if (stdin != null)
            {
                _ = Task.Run(async () => {

                    try
                    {
                        using var s = process.StandardInput.BaseStream;

                        await stdin.CopyToAsync(s).ConfigureAwait(false);
                        await s.FlushAsync().ConfigureAwait(false);
                    }
                    catch (IOException)
                    { }
                    finally
                    {
                        stdin.Dispose();
                        copyTasks.Release();
                    }
                });
            }
            else
            {
                process.StandardInput.Close();
                copyTasks.Release();
            }

            if (stdout != null)
            {
                _ = Task.Run(async () => {
                    try
                    {
                        using var s = process.StandardOutput.BaseStream;

                        await s.CopyToAsync(stdout).ConfigureAwait(false);
                        await stdout.FlushAsync().ConfigureAwait(false);
                    }
                    catch (IOException)
                    { }
                    finally
                    {
                        stdout.Dispose();
                        copyTasks.Release();
                    }
                });
            }
            else
            {
                copyTasks.Release();
            }

            if (stderr != null)
            {
                _ = Task.Run(async () => {
                    try
                    {
                        using var s = process.StandardError.BaseStream;

                        await s.CopyToAsync(stderr).ConfigureAwait(false);
                        await stderr.FlushAsync().ConfigureAwait(false);
                    }
                    catch (IOException)
                    { }
                    finally
                    {
                        stderr.Dispose();
                        copyTasks.Release();
                    }
                });
            }
            else
            {
                copyTasks.Release();
            }
        }
    }
}
