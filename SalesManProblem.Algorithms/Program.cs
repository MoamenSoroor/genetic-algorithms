using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SalesManProblem.Algorithms
{
    class Program
    {

        static void Main(string[] args)
        {
            Randomizer randomizer = new Randomizer();
            GNAParameters pars = new GNAParameters(
                iterations: 3,
                populationSize: 10,
                crossoverRate: 0.80,
                mutationRate: 0.90,
                elitismRate: 0.80,
                allPositions: GenerateRandomPoints(randomizer,20,100),
                randomizer:randomizer,
                roundsPerIteration: 100
                );

            GNAlgorithm algorithm = new GNAlgorithm(pars);

            Console.WriteLine("Travelling Salesman Problem Solution with Genetic Algorithm");
            Console.WriteLine("".PadLeft(80, '='));
            Console.WriteLine("wait...");
            var results = Measure(()=> algorithm.Execute());

            Console.Clear();
            Console.WriteLine("Travelling Salesman Problem Solution with Genetic Algorithm");
            Console.WriteLine("".PadLeft(80, '='));
            Console.WriteLine($" total Time: {results.ms}");
            Console.WriteLine($" Avg path length : {results.result.AveragePathLength}");
            Console.WriteLine($" best path length : {results.result.MapPath.PathLength}");
            Console.WriteLine($" best path:");
            var pos = string.Join(Environment.NewLine, results.result.MapPath.Positions.Select((v, i) => $"City at: ( {v.X}, {v.Y} )"));
            Console.WriteLine(pos);
            Console.WriteLine("".PadLeft(80, '='));
        }

        public static ImmutableList<Point> GenerateRandomPoints(Randomizer randomizer,int count,int max)
        {
            return randomizer.RandomUniqueSequence(count, max)
                .Zip(randomizer.RandomUniqueSequence(count, max))
                .Select(pair => new Point(pair.First, pair.Second)).ToImmutableList();
        }


        public static long Measure(Action action)
        {
            Stopwatch watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }


        public static (T result,long ms) Measure<T>(Func<T> function)
        {
            Stopwatch watch = Stopwatch.StartNew();
            T result = function();
            watch.Stop();
            return (result, watch.ElapsedMilliseconds);
        }

        public static void TestRandomizer()
        {
            Randomizer randomizer = new Randomizer();

            Stopwatch watch = Stopwatch.StartNew();
            Console.WriteLine(string.Join(", ", randomizer.RandomUniqueSequence(10, 10)));
            watch.Stop();

            Console.WriteLine($"{watch.ElapsedMilliseconds} ms");
        }
    }
}
