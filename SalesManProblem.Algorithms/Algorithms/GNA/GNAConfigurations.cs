using SalesManProblem.Algorithms.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManProblem.Algorithms.Utils;

namespace SalesManProblem.Algorithms.GNA
{


    public class GNAConfigurations
    {


        #region Top Level Configuration Functions


        public static Func<Map,GNAResult> Configure(GNAChoices choices, GNAOptions options)
        {
            var executeIteration = AdjustIteration(choices, options);
            return (Map map) =>
            {
                Stopwatch watch = Stopwatch.StartNew();
                var results = Enumerable.Range(1, options.Iterations)
                    // consume the AdjustIteration return function
                    .Select((round) => executeIteration(map))
                    .ToList();
                var min = results.Min(v => v.Path.PathLength);
                var selected = results.FirstOrDefault(res => res.Path.PathLength == min);

                watch.Stop();
                return new GNAResult((int)results.Average(iter => (int)iter.Path.PathLength), selected.Path, watch.Elapsed, selected.Elapsed);
            };
        }




        /// <summary>
        /// returns function that can execute one iteration of the algorithm
        /// </summary>
        /// <param name="choices"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Func<Map, GNAIterationResult> AdjustIteration(GNAChoices choices, GNAOptions options)
        {
            var nextGeneration = ConfigureGenerationPath(choices, options);
            return (Map map) =>
                ConfigureIteration(options, () => Generation.Create(InitialPopulation(map, options)), nextGeneration);
        }

        public static GNAIterationResult ConfigureIteration(GNAOptions options, Func<Generation> initialGeneration, Func<Generation, Generation> nextGenerationPath)
        {

            Stopwatch watch = Stopwatch.StartNew();
            var gen = initialGeneration();

            foreach (var item in Enumerable.Range(0, options.PopulationSize))
                gen = nextGenerationPath(gen);


            watch.Stop();
            return new GNAIterationResult(gen.BestPath, watch.Elapsed);
        }

        public static Func<Generation, Generation> ConfigureGenerationPath(GNAChoices choices, GNAOptions options)
        {


            var selectNewCandidate = GetAllSelection()
                .FirstOrDefault((v) => v.Item1 == choices.SelectionChoice)
                .Item2;


            var crossOverPairCandidates = GetAllCrossOver()
                .FirstOrDefault((v) => v.Item1 == choices.CrossOverChoice)
                .Item2;


            var mutationCandidate = GetAllMutation()
                .FirstOrDefault((v) => v.Item1 == choices.MutationChoice)
                .Item2;



            var elitism = GetAllElitism()
                .FirstOrDefault((v) => v.Item1 == choices.ElitismChoice)
                .Item2.ReduceParameters(options);


            var fitness = GetAllFitness()
                .FirstOrDefault((v) => v.Item1 == choices.FitnessChoice)
                .Item2.ReduceParameters(options);


            var part1 = fitness.Compose(elitism);


            Func<ImmutableList<Candidate>, GNAOptions, Func<ImmutableList<Candidate>, Candidate>, ImmutableList<Candidate>> selection = GenerateNewCandidates;

            Func<ImmutableList<Candidate>, GNAOptions, Func<(Candidate firstInput, Candidate secondInput), (Candidate first, Candidate second)>, ImmutableList<Candidate>> crossOver = CrossOver;

            Func<ImmutableList<Candidate>, GNAOptions, Func<Candidate, Candidate>, ImmutableList<Candidate>> mutation = Mutation;


            var newCandidates = selection.ReduceParameters(selectNewCandidate).ReduceParameters(options)
                .Compose(crossOver.ReduceParameters(crossOverPairCandidates).ReduceParameters(options))
                .Compose(mutation.ReduceParameters(mutationCandidate).ReduceParameters(options));



            return (gen) =>
            {
                var eliteCandidates = part1(gen.Candidates);
                return Generation.Create(eliteCandidates.Concat(newCandidates(eliteCandidates)).ToImmutableList());
            };

        }


        public static ImmutableList<Candidate> InitialPopulation(Map map, GNAOptions options)
        {
            return Enumerable.Range(1, options.PopulationSize)
                .Select(v => Candidate.Create(MapPath.CreateRandom(map))).ToImmutableList();
        }

        #endregion


        #region Path to Configure Generation of new Generation


        // fitness 
        public static ImmutableList<Candidate> Fitness(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            var worstLength = candidates.Max(c => c.Path.PathLength);
            var newCandidates = candidates
                .AsParallel()
                .Select((c) => Candidate.Create(c.Path, worstLength - c.Path.PathLength))
                .ToImmutableList();

            return newCandidates;
        }

        public static ImmutableList<Candidate> Fitness2(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            var newCandidates = candidates
                .AsParallel()
                .Select((c) => Candidate.Create(c.Path, 1/c.Path.PathLength))
                .ToImmutableList();

            return newCandidates;
        }

        // elitism 
        public static ImmutableList<Candidate> Elitism(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            var eliteCandiates = candidates
                .OrderByDescending(e => e.Fitness)
                .Take((int)(options.ElitismRate * options.PopulationSize))
                //.Take(elitismFunction(candidates))
                .ToImmutableList();

            return eliteCandiates;
        }




        // take path
        public static ImmutableList<Candidate> GenerateNewCandidates(ImmutableList<Candidate> candidates, GNAOptions options, Func<ImmutableList<Candidate>, Candidate> selectionFunction)
        {
            return Enumerable.Range(0, options.PopulationSize - candidates.Count)
                .Select(c => selectionFunction(candidates))
                .ToImmutableList();

        }


        public static ImmutableList<Candidate> CrossOver(ImmutableList<Candidate> candidates, GNAOptions options, Func<(Candidate firstInput, Candidate secondInput), (Candidate first, Candidate second)> crossOverPair)
        {
            // crossover with crossOverRate
            var newCandidates = candidates.Take(candidates.Count / 2)
                .Zip(candidates.Skip(candidates.Count / 2))
                .AsParallel()
                .Select(pair => RandomGenerator.NextDouble() < options.CrossOverRate ?
                crossOverPair(pair) : pair).SelectMany(v => new List<Candidate> { v.Item1, v.Item2 }).ToImmutableList();

            return newCandidates;

        }


        public static ImmutableList<Candidate> Mutation(ImmutableList<Candidate> candidates, GNAOptions options, Func<Candidate, Candidate> applyMutation)
        {
            var newCandidates = candidates
                .AsParallel()
                .Select(c => RandomGenerator.NextDouble() < options.MutationRate ?
                applyMutation(c) : c).ToImmutableList();
            return newCandidates;
        }

        #endregion


        #region Function that i pass to the Higher Order Functions (HOF) to configure ConfigureGenerationPath


        public static Candidate ApplyPlacementMutation(Candidate candidate, Func<int, int> randomFunc)
        {
            var index = randomFunc(candidate.Path.CityCount);
            var newIndex = randomFunc(candidate.Path.CityCount);
            return Candidate.Create(MapPath.Create(candidate.Path, index, newIndex));
        }


        public static (Candidate first, Candidate second) ApplyCrossOver((Candidate first, Candidate second) pair, Func<int, int, int> randomFunc)
        {
            (Candidate first, Candidate second) = pair;

            int cityCount = first.Path.CityCount;

            int start = randomFunc(0, first.Path.CityCount - 1);
            int end = randomFunc(start + 1, first.Path.CityCount);
            int count = end - start + 1;


            var firstPos = first.Path.Positions;
            var secondPos = first.Path.Positions;


            var indices = Enumerable.Range(start, count).ToImmutableList();

            var firstSwaps = indices.Select(index => (Index: index,
            NewIndex: first.Path.Positions.IndexOf(second.Path.Positions[index]))
                ).Where(tuple => tuple.NewIndex != -1 && tuple.Index != tuple.NewIndex)
                .ToImmutableList();

            var secondSwaps = indices.Select(index => (Index: index,
            NewIndex: second.Path.Positions.IndexOf(first.Path.Positions[index]))
                ).Where(tuple => tuple.NewIndex != -1 && tuple.Index != tuple.NewIndex)
                .ToImmutableList();

            return (Candidate.Create(MapPath.Create(first.Path, firstSwaps)), Candidate.Create(MapPath.Create(first.Path, firstSwaps)));

        }



        public static Candidate RouletteWheelSelection(ImmutableList<Candidate> candidates, Func<double, double> randomFunc)
        {


            double totalFitness = candidates.Sum(c => c.Fitness);
            double randomValue = randomFunc(totalFitness);

            var selected = candidates
                .SkipWhile(c =>
                {
                    // this is a closure value change ,
                    // i make it for simplicity, againest complex aggregate 
                    randomValue -= c.Fitness;
                    return randomValue > 0;
                }).FirstOrDefault()
                ?? candidates.LastOrDefault();


            return selected;

        }


        public static Candidate RandomCandidateSelection(ImmutableList<Candidate> candidates, Func<int, int> randomFunc)
        {
            int randomIndex = randomFunc(candidates.Count);
            return candidates[randomIndex]; // NOTE That [] of ImmutableList is Log(n) not n
        }

        #endregion


        #region Adapters to compact the parameters of the functions that i pass to HOF

        public static Func<Candidate, Candidate> ApplyPlacementMutationAdapter()
        {
            return (candidate) => ApplyPlacementMutation(candidate, RandomGenerator.NextInt);
        }


        public static Func<(Candidate first, Candidate second), (Candidate first, Candidate second)> ApplyCrossOverAdapter()
        {
            return (pair) => ApplyCrossOver(pair, RandomGenerator.NextInt);
        }



        public static Func<ImmutableList<Candidate>, Candidate> RouletteWheelSelectionAdapter()
        {
            return (candidates) => RouletteWheelSelection(candidates, RandomGenerator.NextDouble);

        }

        public static Func<ImmutableList<Candidate>, Candidate> RandomCandidateSelectionAdapter()
        {
            return (candidates) => RandomCandidateSelection(candidates, RandomGenerator.NextInt);

        }


        #endregion



        #region List of Func


        public static List<(FitnessChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)> GetAllFitness()
        {
            return new List<(FitnessChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)>()
            {
                (FitnessChoice.Default, Fitness),
                (FitnessChoice.Fitness2, Fitness2),
            };
        }


        public static List<(ElitismChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)> GetAllElitism()
        {
            return new List<(ElitismChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)>()
            {
                (ElitismChoice.Default, Elitism),
            };
        }


        public static List<(MutationChoice, Func<Candidate, Candidate>)> GetAllMutation()
        {
            return new List<(MutationChoice choice, Func<Candidate, Candidate> func)>()
            {
                (MutationChoice.Default,ApplyPlacementMutationAdapter()),
            };
        }


        public static List<(CrossOverChoice, Func<(Candidate first, Candidate second), (Candidate first, Candidate second)>)> GetAllCrossOver()
        {
            return new List<(CrossOverChoice choice, Func<(Candidate first, Candidate second), (Candidate first, Candidate second)> func)>()
            {
                (CrossOverChoice.Default,ApplyCrossOverAdapter()),
            };
        }


        public static List<(SelectionChoice, Func<ImmutableList<Candidate>, Candidate>)> GetAllSelection()
        {
            return new List<(SelectionChoice choice, Func<ImmutableList<Candidate>, Candidate>)>()
            {
                (SelectionChoice.Default,RouletteWheelSelectionAdapter()),
            };
        }


        #endregion


    }









}
