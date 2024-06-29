using BenchmarkDotNet.Attributes;
using HomeWork.Comparers;
using HomeWork.Comparisons;
using HomeWork.Levenshtein;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork.Benchmarks;

public class StringsComparisonBenchmark
{
    private StringsComparison _stringsComparison;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        RegisterServices(services);
        var serviceProvider = services.BuildServiceProvider();
        var stringComparer = new StringsComparer(
            serviceProvider.GetService<ILevenshteinDistance>()
        );
        _stringsComparison = new StringsComparison(stringComparer);
    }

    [Benchmark]
    public void Run()
    {
        _stringsComparison.RunComparisons();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<ILevenshteinDistance, OldLevenshteinDistance>();
    }
}
