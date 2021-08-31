using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // y = w1*x1 + w2 * x2 + w3 * x3 + w4 * x4 + w5 * x5 + w6 * x6
            // y = 4 * w1 - 2 * w2 + 7 * w3 + 5 * w4 + 11 * w5 + w6
            // y = 44.1

            //var expected = 44.1;
            //var sample = new double[] { 4, -2, 7, 5, 11, 6 };

            ProblemSample problem = new ProblemSample(
                new double[] { 4, -2, 7, 5, 11, 6 },
                44.1, (a, b) => Math.Abs(a - b));

            List<Chromosome> chromos = new List<Chromosome>()
            {
                new Chromosome( new double []{ 2.4, 0.7, 8, -2, 5, 11 } ),
                new Chromosome( new double []{ -0.4, 2.7, 5, -1, 7, 0.1 }),
                new Chromosome( new double []{ 4, 7 , 12 , 6.1 , 1.4, -4 }),
                new Chromosome( new double []{ 3.1, 4, 0, 2.4, 4.8, 0 }),
                new Chromosome( new double []{ -2, 3, -7, 6, 3, 3}),
            };

            var result = Solve(20,problem, chromos);

            var txt = string.Join(
                Environment.NewLine,
                result.Select(r => 
                    $"Error: {problem.Error(r.Gene):10}, Chromo: {string.Join(", ", r.Gene)}"));



            Console.WriteLine(txt);

        }

        static IEnumerable<Chromosome> Solve(int iterations, ProblemSample problem, IEnumerable<Chromosome> chromosomes)
        {

            return Enumerable.Range(1, iterations)
                .Select(c => chromosomes)
                .Aggregate((chroms, value) =>
            {
                var result = chroms.OrderByDescending(s => problem.Fit(s.Gene))
                .Take(chroms.Count() / 2);

                return result.SkipLast(1).Select((v, i) =>
                    new
                    {
                        Current = v,
                        Next = result.Skip(i+1).First()
                    })
                .Append(new {Current=result.First(), Next=result.Last() })
                .SelectMany(v =>
                new List<Chromosome>()
                { v.Current,
                    v.Current.NewChromosome(v.Next)
                }).ToList();

            }).ToList();

        }

    }


    class Chromosome
    {
        public IEnumerable<double> Gene { get; set; }
        public Func<double, double> Mutation { get; }

        private Random random = new Random();

        public Chromosome(IEnumerable<double> gene, Func<double, double> mutation)
        {
            Gene = gene;
            Mutation = mutation;
        }

        public Chromosome(IEnumerable<double> gene)
        {
            Gene = gene;
            Mutation = (x) => x / 2;
        }

        public Chromosome NewChromosome(Chromosome chromosome)
        {
            var gene = this.Gene.Take(this.Gene.Count() / 2)
                .Concat(chromosome.Gene.TakeLast(chromosome.Gene.Count() / 2)).ToList();
            int index = random.Next(gene.Count);
            //int index = gene.Count / 4;
            gene[index] = Mutation(gene[index]);
            return new Chromosome(gene);
        }







    }


    class ProblemSample
    {
        public IEnumerable<double> Sample { get; set; }

        public double ExpectedResult { get; set; }
        public Func<double, double, double> ErrorFunc { get; private set; }
            = (a, b) => Math.Abs(a - b);

        public ProblemSample(IEnumerable<double> data, double expectedResult, Func<double, double, double> errorFunction)
        {
            Sample = data;
            ExpectedResult = expectedResult;
            this.ErrorFunc = errorFunction;
        }

        public double Result(IEnumerable<double> chromosome)
        {
            Guard.Against.Null(chromosome, nameof(chromosome), "chromosome can't be null");
            Guard.Against.EqualLength(chromosome, Sample, nameof(chromosome), "chromosome doesn't has the same length");
            return Sample.Zip(chromosome, (d, s) => d * s).Sum();
        }

        public double Error(IEnumerable<double> chromosome)
        {
            return ErrorFunc(Result(chromosome), ExpectedResult);
        }

        public double Fit(IEnumerable<double> chromosome)
        {
            return 1 / Error(chromosome);
        }










    }



    public static class ExtensionMethods
    {
        public static void EqualLength<T>(this IGuardClause guard, IEnumerable<T> source1, IEnumerable<T> source2, string name, string message)
        {
            if (source1.Count() != source2.Count())
                throw new Exception($"{name}: {message}");
        }
    }


}
