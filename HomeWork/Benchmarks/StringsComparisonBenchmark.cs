using BenchmarkDotNet.Attributes;
using HomeWork.Calculators;
using HomeWork.Comparers;
using HomeWork.Comparisons;
using HomeWork.Levenshtein;
using HomeWork.Utilities;
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
            serviceProvider.GetService<ILevenshteinDistance>(),
            serviceProvider.GetService<IStringManipulator>(),
            serviceProvider.GetService<ISimilarityCalculator>(),
            serviceProvider.GetService<IStringPreprocessor>()
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
        services.AddSingleton<IStringManipulator, StringManipulator>();
        services.AddSingleton<ISimilarityCalculator, SimilarityCalculator>();
        services.AddSingleton<IStringPreprocessor, CaseInsensitivePreprocessor>();
    }
}
