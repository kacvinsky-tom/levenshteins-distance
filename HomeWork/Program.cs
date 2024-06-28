using System;
using System.Diagnostics;
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
            var services = new ServiceCollection();
            RegisterServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var stringComparer = new StringsComparer(
                serviceProvider.GetService<ILevenshteinDistance>()
            );
            var stringsComparison = new StringsComparison(stringComparer);
            var ws = new Stopwatch();

            Console.WriteLine("Start");
            ws.Restart();
            stringsComparison.RunComparisons();
            ws.Stop();
            Console.WriteLine($"Finished in: {ws.ElapsedTicks}");
            Console.WriteLine($"Finished in: {ws.Elapsed}");
            Console.ReadKey();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ILevenshteinDistance, OldLevenshteinDistance>();
        }
    }
}
