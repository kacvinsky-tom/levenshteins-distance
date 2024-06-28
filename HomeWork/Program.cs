using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using HomeWork.Benchmarks;
using HomeWork.Comparers;
using HomeWork.Comparisons;
using HomeWork.Levenshtein;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork
{
    class Program
    {
        private static void Main()
        {
            // BenchmarkDotNet Benchmark (For more accurate benchmarking)
            BenchmarkRunner.Run<StringsComparisonBenchmark>();
            
            // StopWatch Benchmark (For faster benchmarking)
            
            /*
            var stringComparisonBenchmark = new StringsComparisonBenchmark();
            stringComparisonBenchmark.Setup();
            var ws = new Stopwatch();

            Console.WriteLine("Start");
            ws.Restart();
            stringComparisonBenchmark.Run();
            ws.Stop();
            
            Console.WriteLine($"Finished in: {ws.ElapsedTicks}");
            Console.WriteLine($"Finished in: {ws.Elapsed}");
            */
            
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ILevenshteinDistance, OldLevenshteinDistance>();
        }
    }
}
