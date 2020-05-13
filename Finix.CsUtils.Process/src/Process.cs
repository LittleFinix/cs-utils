using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Finix.CsUtils
{
    public class Process
    {
        private string? executable = null;
        private FileInfo? executableFile = null;

        public Process(string executable, params ProcessArgument[] args)
        {
            Executable = executable;
            Arguments.AddRange(args);
        }

        public Process(FileInfo executable, params ProcessArgument[] args)
        {
            ExecutableFile = executable;
            Arguments.AddRange(args);
        }

        public string Executable
        {
            get => executable ?? ExecutableFile.FullName;
            set
            {
                executableFile = null;
                executable = value;
            }
        }

        public FileInfo ExecutableFile
        {
            get => executableFile ?? new FileInfo(new ProcessStartInfo(Executable).FileName);
            set
            {
                executableFile = value;
                executable = null;
            }
        }

        public List<ProcessArgument> Arguments { get; set; } = new List<ProcessArgument>();

        public ProcessArgument AddArgument(object param, object? param_arg = null)
        {
            var arg = new ProcessArgument(param, param_arg);
            Arguments.Add(arg);
            return arg;
        }

        public Task<int> Execute(Stream? stdin = null, Stream? stdout = null, Stream? stderr = null)
        {
            var args = Arguments.SelectMany((args) => args.ToArray());

            return ProcessUtil.InvokeAsync(Executable, args, stdin, stdout, stderr);
        }
    }
}
