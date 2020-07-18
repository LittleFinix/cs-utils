using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using Finix.CsUtils;

namespace Finix.CsUtils.DataStructures.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            var max = 100;
            using var pages = new MemoryPageAccessor<BTreeNode<long, long>.Data>(1000);

            var ref1 = pages.GetReference(0);
            var ref2 = pages.GetReference(0);

            ref1.Value.Value = 10;
            ref2.Value.Value = 100;

            if (ref1.Value.Value != 100)
                throw new Exception();

            var btree = new BTree<long, long>(pages);

            for (var i = 0L; i < max; i += 2)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"Adding {i}");

                btree.Add(i + 1, (i + 1) * 10);
                Visualizer.Visualize(btree, $"tree_{i}");
                btree.Add(i, i * 10);
                Visualizer.Visualize(btree, $"tree_{i + 1}");
            }

            var rand = new Random(10);
            for (var i = 0; i < 10; i++)
            {
                var idx = (long) rand.Next(max - 1);
                Console.WriteLine($"{idx}: {btree[idx]}");
            }

            return;

            for (var i = 3; i <= 7; i++)
            {
                RunRandomIterations((int)Math.Pow(10, i), 10000);
                RunRandomIterations(5 * (int)Math.Pow(10, i), 10000);
            }
        }

        static void RunLinearIterations(int max, int accesses)
        {
            using var paged_data = new MemoryPageAccessor<long>(4000 / sizeof(long));
            using var pages = new MemoryPageAccessor<BinaryHeap<int, long>.Data>(4000 / sizeof(long));

            var heap = new BinaryHeap<int, long>(pages);
            var dict = new Dictionary<int, long>();
            var list = new List<long>();

            TimeSpan FillMPA(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0U; i < (ulong) max; i++)
                {
                    var idx = paged_data.FindFreeIndex();
                    var page = paged_data.GetPageFor(idx);
                    page.Set(idx);
                    page[idx] = i;
                }
                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem + paged_data.Pages.Sum(p => (long)p.Value.AllocatedBytes);

                return timer.Elapsed;
            }

            TimeSpan FillDict(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0; i < max; i++)
                {
                    dict[i] = i;
                }
                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem;

                return timer.Elapsed;
            }

            TimeSpan FillList(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0; i < max; i++)
                {
                    list.Add(i);
                }
                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem;

                return timer.Elapsed;
            }

            TimeSpan RunMPA(Stopwatch timer)
            {
                var rand = new Random(10);
                timer.Restart();
                for (var i = 0; i < accesses; i++)
                {
                    var val = (ulong) (i % max); // rand.Next(max);
                    // var val = heap[i];
                    var str = paged_data.GetPageFor(val)[val];
                    GC.KeepAlive(str);
                    // Console.WriteLine($"Element at index {val}: {str}");
                }
                timer.Stop();

                // Console.WriteLine($"MemoryPageAccessor: {timer.Elapsed}");
                return timer.Elapsed;
            }

            TimeSpan RunDictionary(Stopwatch timer)
            {
                var rand = new Random(10);
                timer.Restart();
                for (var i = 0; i < accesses; i++)
                {
                    var val = (i % max); // rand.Next(max);
                    // var val = heap[i];
                    var str = dict[val];
                    GC.KeepAlive(str);
                    // Console.WriteLine($"Element at index {val}: {str}");
                }
                timer.Stop();

                // Console.WriteLine($"Dictionary: {timer.Elapsed}");
                return timer.Elapsed;
            }

            TimeSpan RunList(Stopwatch timer)
            {
                var rand = new Random(10);
                timer.Restart();
                for (var i = 0; i < accesses; i++)
                {
                    var val = (i % max); // rand.Next(max);
                    // var val = heap[i];
                    var str = list[val];
                    GC.KeepAlive(str);
                    // Console.WriteLine($"Element at index {val}: {str}");
                }
                timer.Stop();

                // Console.WriteLine($"List: {timer.Elapsed}");
                return timer.Elapsed;
            }

            var timer = new Stopwatch();

            var mpaTimes = new List<TimeSpan>(100);
            var dictTimes = new List<TimeSpan>(100);
            var listTimes = new List<TimeSpan>(100);

            var mpaAlloc = FillMPA(timer, out var mpaMem).TotalMilliseconds;
            var dictAlloc = FillDict(timer, out var dictMem).TotalMilliseconds;
            var listAlloc = FillList(timer, out var listMem).TotalMilliseconds;

            for (var i = 0; i < 100; i++)
            {
                // Console.WriteLine($"\n === Iteration {i:00} ===");

                GC.TryStartNoGCRegion(1000 + accesses * 100);

                mpaTimes.Add(RunMPA(timer));
                dictTimes.Add(RunDictionary(timer));
                listTimes.Add(RunList(timer));

                GC.EndNoGCRegion();
            }

            //Console.WriteLine($"\n\n *** Results ***");

            Console.Write($"{max},{mpaAlloc},{mpaMem}");
            PrintResults($"MPA {max} values", mpaTimes);
            Console.Write($",{dictAlloc},{dictMem}");
            PrintResults($"DICT {max} values", dictTimes);
            Console.Write($",{listAlloc},{listMem}");
            PrintResults($"LIST {max} values", listTimes);
            Console.WriteLine();
        }

        static void RunRandomIterations(int max, int accesses)
        {
            using var paged_data = new MemoryPageAccessor<long>(4000 / sizeof(long));
            using var pages = new MemoryPageAccessor<BinaryHeap<int, long>.Data>(4000 / sizeof(long));

            var heap = new BinaryHeap<int, long>(pages);
            var dict = new Dictionary<int, long>();
            var list = new List<long>();

            TimeSpan FillMPA(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0U; i < (ulong) max; i++)
                {
                    var idx = (ulong) rand.Next(max);
                    var page = paged_data.GetPageFor(idx);
                    page.Set(idx);
                    page[idx] = i;
                }

                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem + paged_data.Pages.Sum(p => (long) p.Value.AllocatedBytes);

                return timer.Elapsed;
            }

            TimeSpan FillDict(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0; i < max; i++)
                {
                    var idx = rand.Next(max);
                    dict[idx] = i;
                }
                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem;

                return timer.Elapsed;
            }

            TimeSpan FillList(Stopwatch timer, out long mem)
            {
                var rand = new Random(10);

                mem = GC.GetTotalMemory(true);
                timer.Restart();

                for (var i = 0; i < max; i++)
                {
                    var idx = rand.Next(max);

                    for (int j = list.Count; j <= idx; j++)
                    {
                        list.Add(-1);
                    }

                    list[idx] = i;
                }
                timer.Stop();
                mem = GC.GetTotalMemory(true) - mem;

                return timer.Elapsed;
            }

            var timer = new Stopwatch();

            var mpaAlloc = FillMPA(timer, out var mpaMem).TotalMilliseconds;
            var dictAlloc = FillDict(timer, out var dictMem).TotalMilliseconds;
            var listAlloc = FillList(timer, out var listMem).TotalMilliseconds;

            //Console.WriteLine($"\n\n *** Results ***");

            Console.Write($"{max},{mpaAlloc},{mpaMem}");
            Console.Write($",{dictAlloc},{dictMem}");
            Console.Write($",{listAlloc},{listMem}");
            Console.WriteLine();
        }

        static void PrintResults(string name, ICollection<TimeSpan> times)
        {
            var best = times.Min(time => time.TotalMilliseconds);
            var worst = times.Max(time => time.TotalMilliseconds);
            var avg = times.Average(time => time.TotalMilliseconds);
            var med = times.OrderBy(time => time.TotalMilliseconds).ElementAt(times.Count / 2).TotalMilliseconds;

            //Console.WriteLine($"{name}: best {best}ms, worst {worst}ms, avg {avg}ms");

            // Console.Write($",{best},{worst},{avg}");
            Console.Write($",{med}");
        }
    }
}
