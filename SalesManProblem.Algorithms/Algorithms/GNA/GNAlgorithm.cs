using Ardalis.GuardClauses;
using MoreLinq;
using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.GNA
{

    public class GNAOptions
    {
        private readonly int iterations;
        private readonly int roundsPerIteration;
        private readonly int populationSize;
        private readonly double crossoverRate;
        private readonly double mutationRate;
        private readonly double elitismRate;

        public GNAOptions(int iterations, int populationSize, double crossoverRate, double mutationRate, double elitismRate, int roundsPerIteration)
        {
            this.iterations = iterations;
            this.populationSize = populationSize;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.elitismRate = elitismRate;
            this.roundsPerIteration = roundsPerIteration;
        }

        public int Iterations => iterations;

        public int PopulationSize => populationSize;

        public double CrossOverRate => crossoverRate;

        public double MutationRate => mutationRate;

        public double ElitismRate => elitismRate;

        public int RoundsPerIteration => roundsPerIteration;
    }

    public class GNAlgorithm
    {

        private readonly Func<Map,GNAResult> execute;
        
        public GNAlgorithm(GNAChoices choices,GNAOptions options)
        {

            execute = GNAConfigurations.Configure(choices, options);
        }

        public GNAResult Execute(Map map)
        {
            return execute(map);
                
        }


    }

    public class Generation
    {

        public static Generation Create(ImmutableList<Candidate> candidates)
        {
            return new Generation(candidates);
        }


        private Generation(ImmutableList<Candidate> candidates)
        {
            Candidates = candidates;
            BestPathLength = (int)Candidates.Min(c => c.Path.PathLength);
            BestPath = Candidates.FirstOrDefault(v => v.Path.PathLength >= BestPathLength).Path;
        }


        public ImmutableList<Candidate> Candidates { get; }
        public int GenerationNumber { get; }
        public int BestPathLength { get; }
        public MapPath BestPath { get; }

        
    }

    public class GNAIterationResult
    {


        public GNAIterationResult(MapPath path, TimeSpan elapsed)
        {
            Path = path;
            Elapsed = elapsed;
        }

        //public int TotalPathLength { get => (int)Path.PathLength; }
        //public IImmutableList<Point> Positions { get => Path.Positions; }

        public MapPath Path { get; }
        public TimeSpan Elapsed { get; }
    }

    public class GNAResult
    {
        private int averagePathLength;
        //private readonly int totalPathLength;
        //private IImmutableList<Point> positions;
        private MapPath mapPath;

        //public GNAResult(int averagePathLength, int totalPathLength, IImmutableList<Point> positions)
        //{
        //    this.averagePathLength = averagePathLength;
        //    //this.totalPathLength = totalPathLength;
        //    //this.positions = positions;

        //}

        public GNAResult(int averagePathLength, MapPath path, TimeSpan totalElapsed, TimeSpan iterationElapsed)
        {
            this.averagePathLength = averagePathLength;
            this.mapPath = path;
            TotalElapsed = totalElapsed;
            IterationElapsed = iterationElapsed;
            //this.totalPathLength = (int)path.PathLength;
            //this.positions = path.Positions;
        }

        public int AveragePathLength { get => averagePathLength; }
        //public int TotalPathLength { get => totalPathLength; }
        //public IImmutableList<Point> Positions { get => positions; }
        public MapPath MapPath { get => mapPath; }
        public TimeSpan TotalElapsed { get; }
        public TimeSpan IterationElapsed { get; }
    }

    public class Candidate
    {
        /// <summary>
        /// create candidate from a path (shallow copy) with fitness equals 0.0
        /// </summary>
        /// <param name="path"></param>
        /// <returns>new candidate (shallow copy)</returns>
        public static Candidate Create(MapPath path)
        {
            return new Candidate(path, 0.0);
        }

        /// <summary>
        /// create candidate from a path (shallow copy) with new fitness value
        /// </summary>
        /// <param name="path"></param>
        /// <returns>new candidate (shallow copy)</returns>
        public static Candidate Create(MapPath path, double fitness)
        {
            return new Candidate(path, fitness);
        }


        private double fitness;
        private readonly MapPath path;

        public MapPath Path => path; 
        public double Fitness { get => fitness; }


        private Candidate(MapPath path, double fitness)
        {
            this.path = path;
            this.fitness = fitness;
        }



        //public Candidate ApplySwaps(ImmutableList<(int Index, int NewIndex)> firstSwaps)
        //{
        //    var list = new List<Point>(this.path.Positions);

        //    firstSwaps.ForEach(pair =>
        //    {
        //        var temp = list[pair.Index];
        //        list[pair.Index] = list[pair.NewIndex];
        //        list[pair.NewIndex] = temp;
        //    });

        //    return Candidate.Create(MapPath.Create(list.ToImmutableList()));
        //}

        //public Candidate ApplySwap(int index, int newIndex)
        //{
        //    var list = new List<Point>(this.path.Positions);
        //    var temp = list[index];
        //    list[index] = list[newIndex];
        //    list[newIndex] = temp;
        //    return Candidate.Create(MapPath.Create(list.ToImmutableList()));
        //}


    }


    


}
