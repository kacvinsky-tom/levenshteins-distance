using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
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

    public void ExecuteWithDotNetBenchmark()
    {
        BenchmarkRunner.Run<StringsComparisonBenchmark>();
    }

    public void ExecuteWithStopWatches()
    {
        Setup();
        var ws = new Stopwatch();

        Console.WriteLine("Start");
        ws.Restart();
        Run();
        ws.Stop();
        Console.WriteLine($"Finished in: {ws.ElapsedTicks}");
        Console.WriteLine($"Finished in: {ws.Elapsed}");
    }

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
        services.AddSingleton<ILevenshteinDistance, BonusLevenshteinDistance>();
        services.AddSingleton<IStringManipulator, StringManipulator>();
        services.AddSingleton<ISimilarityCalculator, SimilarityCalculator>();
        services.AddSingleton<IStringPreprocessor, CaseInsensitivePreprocessor>();
    }
}
