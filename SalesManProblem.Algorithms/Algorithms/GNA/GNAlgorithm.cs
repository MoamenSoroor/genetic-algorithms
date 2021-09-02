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

    public class GNAParameters
    {
        private readonly int iterations;
        private readonly int roundsPerIteration;
        private readonly int populationSize;
        private readonly double crossoverRate;
        private readonly double mutationRate;
        private readonly double elitismRate;
        private readonly ImmutableList<Point> allPositions;
        private readonly Randomizer randomizer;

        public GNAParameters(int iterations, int populationSize, double crossoverRate, double mutationRate, double elitismRate, ImmutableList<Point> allPositions, Randomizer randomizer, int roundsPerIteration)
        {
            this.iterations = iterations;
            this.populationSize = populationSize;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.elitismRate = elitismRate;
            this.allPositions = allPositions;
            this.randomizer = randomizer;
            this.roundsPerIteration = roundsPerIteration;
        }

        public int Iterations => iterations;

        public int PopulationSize => populationSize;

        public double CrossOverRate => crossoverRate;

        public double MutationRate => mutationRate;

        public double ElitismRate => elitismRate;

        public ImmutableList<Point> AllPositions => allPositions;

        public Randomizer Randomizer => randomizer;

        public int MaxRoundsPerIteration => roundsPerIteration;
    }

    public class GNAlgorithm
    {
        private GNAParameters parameters;
        //private readonly Func<GNAlgorithmParameters, GNAlgorithmResult> execute;

        public GNAlgorithm(GNAParameters parameters, Func<GNAParameters, GNAResult> execute)
        {
            this.parameters = parameters;
            //this.execute = execute;
        }

        public GNAlgorithm(GNAParameters parameters)
        {
            this.parameters = parameters;
            //this.execute = execute;
        }

        public GNAResult Execute()
        {
            Stopwatch watch = Stopwatch.StartNew();
            var results = Enumerable.Range(1, parameters.Iterations)//.AsParallel()
                .Select((round) => ExecuteRound(round, parameters))
                .ToList();
            var min = results.Min(v => v.Path.PathLength);
            var selected = results.FirstOrDefault(res => res.Path.PathLength == min);

            watch.Stop();
            return new GNAResult((int)results.Average(iter => (int)iter.Path.PathLength), selected.Path, watch.Elapsed, selected.Elapsed);
        }


        private GNAIterationResult ExecuteRound(int round, GNAParameters parameters)
        {
            Stopwatch watch = Stopwatch.StartNew();
            var gen = InitGeneration(parameters.PopulationSize, parameters.AllPositions, parameters.Randomizer);

            int maxNoChangeToClose = 100;
            int count = 0;
            var newGeneneration = gen;
            for (int i = 0; i < parameters.MaxRoundsPerIteration; i++)
            {
                newGeneneration = ExecuteGeneration(newGeneneration, parameters);
                if (newGeneneration.BestPathLength <= gen.BestPathLength)
                {
                    gen = newGeneneration;
                    count = 0;
                }
                else
                    count++;

                if (maxNoChangeToClose <= count) break;

            }
            watch.Stop();
            return new GNAIterationResult(gen.BestPath, watch.Elapsed);
        }

        private Generation ExecuteGeneration(Generation generation, GNAParameters parameters)
        {
            // with the longest path
            var worstFitness = generation.Candidates.Max(c => c.Fitness);
            var worstCandidate = generation.Candidates
                //.AsParallel()
                .FirstOrDefault(v => v.Fitness == worstFitness);
            var eliteCandiates = generation.Candidates
                //.AsParallel()
                .Select((c) => new Candidate(c, worstCandidate.Fitness - c.Fitness))
                .OrderByDescending(c => c.Fitness)
                .Take((int)(parameters.ElitismRate * parameters.PopulationSize))
                .ToList();


            // check end conditions


            // selection to generate new generation
            int newCandidatesCount = parameters.PopulationSize - eliteCandiates.Count;
            int selectedPairsCount = newCandidatesCount / 2;

            // the next var is IEnumerable<(Candidate First, Candidate Second)> selectedCandidates
            //var selectedCandidates
            //    = parameters.Randomizer.RandomSequence(selectedPairsCount, selectedPairsCount)
            //    .Zip(parameters.Randomizer.RandomSequence(selectedPairsCount, selectedPairsCount))
            //    .Select(pairs => (First: new Candidate(elitedCandiates[pairs.First]), Second: new Candidate(elitedCandiates[pairs.Second])));

            // Roulette Wheel Selection 
            var selectedCandidates
                = Enumerable.Range(0, selectedPairsCount)
                .Zip(Enumerable.Range(0, selectedPairsCount))
                .AsParallel()
                .Select((pair) => (First: RouletteWheelSelection(eliteCandiates, parameters.Randomizer),
                            Second: RouletteWheelSelection(eliteCandiates, parameters.Randomizer)));


            // crossover with crossOverRate
            var newCandidates = selectedCandidates
                .AsParallel()
                .Select(pair => parameters.Randomizer.DoubleFromZeroToOne() < parameters.CrossOverRate ?
                ApplyCrossOver(pair, parameters.Randomizer) : pair).SelectMany(v => new List<Candidate> { v.Item1, v.Item2 }).ToImmutableList();


            // mutation with mutationRate
            newCandidates = newCandidates
                .AsParallel()
                .Select(c => parameters.Randomizer.DoubleFromZeroToOne() < parameters.MutationRate ?
                ApplyPlacementMutation(c, parameters.Randomizer) : c).ToImmutableList();

            return new Generation(newCandidates, generation.GenerationNumber + 1);

        }




        private Candidate ApplyPlacementMutation(Candidate c, Randomizer randomizer)
        {
            var index = randomizer.IntLessThan(c.Path.CityCount);
            var newIndex = randomizer.IntLessThan(c.Path.CityCount);
            return c.ApplySwap(index, newIndex);
        }

        private (Candidate first, Candidate second) ApplyCrossOver((Candidate first, Candidate second) pair, Randomizer randomizer)
        {
            Guard.Against.InvalidInput(pair,
                "(Candidate first, Candidate second)",
                (pair) => pair.first.Path.CityCount == pair.second.Path.CityCount,
                "(Candidate first, Candidate second) passed to the cross over func has not the same path length");

            (Candidate first, Candidate second) = pair;

            int cityCount = first.Path.CityCount;

            int start = randomizer.IntLessThan(first.Path.CityCount - 1);
            int end = randomizer.IntBetween(start + 1, first.Path.CityCount);
            int count = end - start + 1;


            var firstPos = first.Path.Positions;
            var secondPos = first.Path.Positions;

            //var modifications = firstPos.Select((v, i) => (Index: i,
            //    NewIndex: (i >= start && i <= end ? firstPos.IndexOf(secondPos[i]) : i)))
            //    .Select(tuple=> tuple.Index );

            var indices = Enumerable.Range(start, count).ToImmutableList();

            var firstSwaps = indices.Select(index => (Index: index,
            NewIndex: first.Path.Positions.IndexOf(second.Path.Positions[index]))
                ).Where(tuple => tuple.NewIndex != -1 && tuple.Index != tuple.NewIndex)
                .ToImmutableList();

            var secondSwaps = indices.Select(index => (Index: index,
            NewIndex: second.Path.Positions.IndexOf(first.Path.Positions[index]))
                ).Where(tuple => tuple.NewIndex != -1 && tuple.Index != tuple.NewIndex)
                .ToImmutableList();

            return (first.ApplySwaps(firstSwaps), second.ApplySwaps(secondSwaps));

        }

        private Candidate RouletteWheelSelection(List<Candidate> candidates, Randomizer randomizer)
        {
            Guard.Against.NullOrEmpty(candidates, "candidates", "candidates can't be null or empty seuqnece");
            Guard.Against.Null(randomizer, "randomizer", "randomizer can't be null");

            int totalFitness = candidates.Sum(c => c.Fitness);
            int randomValue = randomizer.IntLessThan(totalFitness);

            var selected = candidates
                .SkipWhile(c =>
                {
                    // this is a closure value change ,
                    // i make it for simplicity, againest complex aggregate 
                    randomValue -= c.Fitness;
                    return randomValue > 0;
                }).FirstOrDefault()
                ?? candidates.LastOrDefault();


            return new Candidate(selected);

        }

        private Generation InitGeneration(int populationSize, ImmutableList<Point> positions, Randomizer randomizer)
        {
            return new Generation(Enumerable.Range(1, populationSize)
                .Select(v => new Candidate(positions, randomizer)).ToImmutableList(), 0);
        }







    }

    public class Generation
    {

        public Generation(ImmutableList<Candidate> candidates, int generationNumber)
        {
            Candidates = candidates;
            GenerationNumber = generationNumber;
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

        private int fitness;

        public MapPath Path { get; }


        public Candidate(MapPath path)
        {
            Path = path;
            fitness = (int)path.PathLength;
        }

        public Candidate(Candidate candidate, int fitness)
        {
            this.Path = new MapPath(candidate.Path);
            this.fitness = fitness;
        }

        public Candidate(ImmutableList<Point> positions, Randomizer randomizer)
        {
            Path = new MapPath(randomizer.RandomUniqueSequence(positions.Count, positions.Count)
                .Select(index => positions[index]).ToImmutableList());
            this.fitness = (int)Path.PathLength;
        }

        public Candidate(Candidate candidate)
        {
            this.Path = new MapPath(candidate.Path);
            this.fitness = candidate.fitness;
        }

        public int Fitness { get => fitness; }

        public Candidate ApplySwaps(ImmutableList<(int Index, int NewIndex)> firstSwaps)
        {
            var list = new List<Point>(this.Path.Positions);

            firstSwaps.ForEach(pair =>
            {
                var temp = list[pair.Index];
                list[pair.Index] = list[pair.NewIndex];
                list[pair.NewIndex] = temp;
            });

            return new Candidate(new MapPath(list.ToImmutableList()));
        }

        internal Candidate ApplySwap(int index, int newIndex)
        {
            var list = new List<Point>(this.Path.Positions);
            var temp = list[index];
            list[index] = list[newIndex];
            list[newIndex] = temp;
            return new Candidate(new MapPath(list.ToImmutableList()));
        }


    }



}
