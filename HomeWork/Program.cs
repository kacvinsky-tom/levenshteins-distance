using HomeWork.Benchmarks;

namespace HomeWork
{
    class Program
    {
        private static void Main()
        {
            var stringComparisonBenchmark = new StringsComparisonBenchmark();

            // BenchmarkDotNet Benchmark (For more accurate benchmarking), Runnable only in Release configuration
            stringComparisonBenchmark.ExecuteWithDotNetBenchmark();

            // StopWatch Benchmark (For faster benchmarking)
            stringComparisonBenchmark.ExecuteWithStopWatches();
        }
    }
}
