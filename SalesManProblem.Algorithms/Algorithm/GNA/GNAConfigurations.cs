using SalesManProblem.Algorithms.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManProblem.Algorithms.Configurations;

namespace SalesManProblem.Algorithms.GNA
{


    public class GNAConfigurations
    {


        #region Top Level Configuration Functions


        /// <summary>
        /// The Most Top Level Higher Order Function that configure the whole process of the 
        /// Genetic algorithm
        /// </summary>
        /// <param name="choices">the Choices that are used to configure the algorithm</param>
        /// <param name="options">the algorithm options that are used to configure the algorithm</param>
        /// <returns>a function can be executed to solve the salesman problem using Genetic Algorithm</returns>
        public static Func<Map,GNAResult> Configure(GNAChoices choices, GNAOptions options)
        {
            var executeIteration = ConfigureIeration(choices, options);
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
                return GNAResult.Create(results.Average(iter => iter.Path.PathLength), selected.Path, watch.Elapsed, selected.Elapsed);
            };
        }




        /// <summary>
        /// Higher Order Function that configure a function that executes one iteration of the algorithm
        /// this function is used inside the <see cref="Configure"/> HOF to execute one iteration 
        /// </summary>
        /// <param name="choices">the Choices that are used to configure the algorithm</param>
        /// <param name="options">the algorithm options that are used to configure the algorithm</param>
        /// <returns></returns>
        public static Func<Map, GNAIterationResult> ConfigureIeration(GNAChoices choices, GNAOptions options)
        {
            var nextGenerationPath = ConfigureNextGenerationPath(choices, options);
            return (Map map) =>
            {

                Stopwatch watch = Stopwatch.StartNew();
                var gen = Generation.Create(InitialPopulation(map, options));

                foreach (var item in Enumerable.Range(0, options.PopulationSize))
                    gen = nextGenerationPath(gen);


                watch.Stop();
                return GNAIterationResult.Create(gen.BestPath, watch.Elapsed);
            };
        }

        /// <summary>
        /// a Higher Order Function that configures a Function that can be used to 
        /// pass a generation <see cref="Generation"/> and get a new one.
        /// </summary>
        /// <param name="choices">the Choices that are used to configure the algorithm</param>
        /// <param name="options">the algorithm options that are used to configure the algorithm</param>
        /// <returns>a function that can be used to generate new Generations </returns>
        public static Func<Generation, Generation> ConfigureNextGenerationPath(GNAChoices choices, GNAOptions options)
        {

            var elitism = GetAllElitism()
                .FirstOrDefault((v) => v.Item1 == choices.ElitismChoice)
                .Item2.ReduceParameters(options);


            var fitness = GetAllFitness()
                .FirstOrDefault((v) => v.Item1 == choices.FitnessChoice)
                .Item2.ReduceParameters(options);


            var eliteCandidatesPath = fitness.Compose(elitism);



            var selectionFunction = GetAllSelection()
                .FirstOrDefault((v) => v.Item1 == choices.SelectionChoice)
                .Item2;


            var crossOverPairCandidates = GetAllCrossOver()
                .FirstOrDefault((v) => v.Item1 == choices.CrossOverChoice)
                .Item2;


            var mutationCandidate = GetAllMutation()
                .FirstOrDefault((v) => v.Item1 == choices.MutationChoice)
                .Item2;






            Func<ImmutableList<Candidate>, GNAOptions, Func<ImmutableList<Candidate>, Candidate>, ImmutableList<Candidate>> selection = SelectFromParents;

            Func<ImmutableList<Candidate>, GNAOptions, Func<(Candidate firstInput, Candidate secondInput), (Candidate first, Candidate second)>, ImmutableList<Candidate>> crossOver = CrossOver;

            Func<ImmutableList<Candidate>, GNAOptions, Func<Candidate, Candidate>, ImmutableList<Candidate>> mutation = Mutation;


            var newCandidatesPath = selection.ReduceParameters(selectionFunction).ReduceParameters(options)
                .Compose(crossOver.ReduceParameters(crossOverPairCandidates).ReduceParameters(options))
                .Compose(mutation.ReduceParameters(mutationCandidate).ReduceParameters(options));



            return (gen) =>
            {
                var eliteCandidates = eliteCandidatesPath(gen.Candidates);
                return Generation.Create(eliteCandidates.Concat(newCandidatesPath(eliteCandidates)).ToImmutableList());
            };

        }


        /// <summary>
        /// a function that is used to generate the initial population
        /// </summary>
        /// <param name="map">map that contains the positions of the cities</param>
        /// <param name="options">GNA options</param>
        /// <returns>initial population</returns>
        public static ImmutableList<Candidate> InitialPopulation(Map map, GNAOptions options)
        {
            return Enumerable.Range(1, options.PopulationSize)
                .Select(v => Candidate.Create(MapPath.CreateRandom(map))).ToImmutableList();
        }

        #endregion


        #region Functions that are composed inside ConfigureGenerationPath Function


        /// <summary>
        /// Fitness Function that calculate the fitness of the each candidate. 
        /// for each candidate: fitness = Longest Path Length - Candidate Path Length
        /// </summary>
        /// <param name="candidates">all candidates to calculate the fitness for them</param>
        /// <param name="options">Genetic Algorithm Options that include all options of the algorithm</param>
        /// <returns>Returns New ImmutableList of Candidates with the new calculated fitness value </returns>
        public static ImmutableList<Candidate> Fitness(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            // problem with that fitness function is that each time it is called, it recalculate worst length
            var worstLength = candidates.Max(c => c.Path.PathLength);
            var newCandidates = candidates
                .AsParallel()
                .Select((c) => Candidate.Create(c.Path, worstLength - c.Path.PathLength))
                .ToImmutableList();

            return newCandidates;
        }



        /// <summary>
        /// Fitness Function that calculate the fitness of the each candidate. 
        /// for each candidate: fitness = 1 / Candidate Path Length
        /// </summary>
        /// <param name="candidates">all candidates to calculate the fitness for them</param>
        /// <param name="options">Genetic Algorithm Options that include all options of the algorithm</param>
        /// <returns>Returns New ImmutableList of Candidates with the new calculated fitness value </returns>
        public static ImmutableList<Candidate> Fitness2(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            var newCandidates = candidates
                .AsParallel()
                .Select((c) => Candidate.Create(c.Path, 1/c.Path.PathLength))
                .ToImmutableList();

            return newCandidates;
        }

        /// <summary>
        /// Elitism Function That is used to order and filter the Candidates within their fitness value.
        /// it takes a number of candidates relative to the ElitismRate that is passed within the GNAOptions
        /// </summary>
        /// <param name="candidates">all candidates to take the best from them </param>
        /// <param name="options">Genetic Algorithm Options that include all options of the algorithm</param>
        /// <returns>Returns New ImmutableList of Candidates that will be parents of the new Candidates </returns>
        public static ImmutableList<Candidate> Elitism(ImmutableList<Candidate> candidates, GNAOptions options)
        {
            var eliteCandiates = candidates
                .OrderByDescending(e => e.Fitness)
                .Take((int)(options.ElitismRate * options.PopulationSize))
                //.Take(elitismFunction(candidates))
                .ToImmutableList();

            return eliteCandiates;
        }




        /// <summary>
        /// this function is a Higher Order Function that is used to Select a number of candidates to use them 
        /// in the generation of new candidates. based on the SelectionFunction paremeter. 
        /// </summary>
        /// <param name="candidates">candidates (parents) to select from them</param>
        /// <param name="options">Genetic Algorithm Options that include all options of the algorithm</param>
        /// <param name="selectionFunction"></param>
        /// <returns></returns>
        public static ImmutableList<Candidate> SelectFromParents(ImmutableList<Candidate> candidates, GNAOptions options, Func<ImmutableList<Candidate>, Candidate> selectionFunction)
        {
            return Enumerable.Range(0, options.PopulationSize - candidates.Count)
                .Select(c => selectionFunction(candidates))
                .ToImmutableList();

        }

        /// <summary>
        /// This Function is Higher Order Function that represents CrossOver process. it takes candidates, 
        /// put each two as pairs , and generate new pairs based on the CrossOverRate (inside GNAOptions)
        /// and it uses pairCrossOver parameter function to generate the new pair
        /// </summary>
        /// <param name="candidates"></param>
        /// <param name="options"></param>
        /// <param name="pairCrossOver"></param>
        /// <returns></returns>
        public static ImmutableList<Candidate> CrossOver(ImmutableList<Candidate> candidates, GNAOptions options, Func<(Candidate firstInput, Candidate secondInput), (Candidate first, Candidate second)> pairCrossOver)
        {
            // crossover with crossOverRate
            var newCandidates = candidates.Take(candidates.Count / 2)
                .Zip(candidates.Skip(candidates.Count / 2))
                .AsParallel()
                .Select(pair => RandomGenerator.NextDouble() < options.CrossOverRate ?
                pairCrossOver(pair) : pair).SelectMany(v => new List<Candidate> { v.Item1, v.Item2 }).ToImmutableList();

            return newCandidates;

        }

        /// <summary>
        /// mutation Higher Order Function that used to mutate new generated candidates from the crossover process
        /// It mutates candidates based on MutationRate(inside GNAOptions) and applyMutation function parameter
        /// </summary>
        /// <param name="candidates"></param>
        /// <param name="options"></param>
        /// <param name="applyMutation">function parameter that is used to mutate the candidates</param>
        /// <returns></returns>
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


        /// <summary>
        /// placement mutation is used as a pure function parameter of <see cref="Mutation"/> Higher Order Function (HOF).
        /// It is not directly passed to Mutation HOF,but it is wrapped inside adapter <see cref="ApplyCrossOverAdapter"/> 
        /// that reduce the parameters of it.
        /// </summary>
        /// <param name="candidate">candidate to generate new mutated one from it</param>
        /// <param name="randomFunc">a function that returns random integer each time i call it. It is passed inside the adapter as a closure <see cref="ApplyCrossOverAdapter"/></param>
        /// <returns>new candidate that has a new path that is mutated</returns>
        public static Candidate ApplyPlacementMutation(Candidate candidate, Func<int, int> randomFunc)
        {
            var index = randomFunc(candidate.Path.CityCount);
            var newIndex = randomFunc(candidate.Path.CityCount);
            return Candidate.Create(MapPath.Create(candidate.Path, index, newIndex));
        }

        /// <summary>
        /// pure Function that is used as a parameter for <see cref="CrossOver"/> Higher Order function
        /// after reducing it's parameters with the adapter <see cref="ApplyCrossOverAdapter"/>
        /// it creates new pair of candidates after applying the CrossOver operation on the passed 
        /// pair of candidates. 
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="randomFunc">a function that returns random integer each time i call it. It is passed inside the adapter as a closure <see cref="ApplyCrossOverAdapter"/></param>
        /// <returns>new pair of candidates</returns>
        public static (Candidate first, Candidate second) PairCrossOver((Candidate first, Candidate second) pair, Func<int, int, int> randomFunc)
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


        /// <summary>
        /// pure Function that is used as a parameter for <see cref="SelectFromParents"/> Higher Order function
        /// after reducing its parameters with the adapter <see cref="RouletteWheelSelectionAdapter"/> 
        /// It selects a candidate based on the Roulette Wheel algorithm.
        /// </summary>
        /// <param name="candidates">candidates to select one of them</param>
        /// <param name="randomFunc">a function that returns random integer each time i call it. It is passed inside the adapter as a closure <see cref="RouletteWheelSelectionAdapter"/></param>
        /// <returns>selected candidate from candidates</returns>
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

        /// <summary>
        /// pure Function that is used as a parameter for <see cref="SelectFromParents"/> Higher Order function
        /// after reducing its parameters with the adapter <see cref="RandomCandidateSelectionAdapter"/> 
        /// It selects a candidate based on the index that is returned form the random Function.
        /// </summary>
        /// <param name="candidates">candidates to select one of them</param>
        /// <param name="randomFunc">a function that returns random integer each time i call it. It is passed inside the adapter as a closure <see cref="RandomCandidateSelectionAdapter"/></param>
        /// <returns>selected candidate from candidates</returns>
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
            return (pair) => PairCrossOver(pair, RandomGenerator.NextInt);
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


        /// <summary>
        /// All Fitness Functions that are used to configure NextGenerationPath HOF inside <see cref="ConfigureNextGenerationPath"/>
        /// </summary>
        /// <returns></returns>
        public static List<(FitnessChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)> GetAllFitness()
        {
            return new List<(FitnessChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)>()
            {
                (FitnessChoice.Default, Fitness),
                (FitnessChoice.Fitness2, Fitness2),
            };
        }


        /// <summary>
        /// All Elitism Functions that are used to configure NextGenerationPath HOF inside <see cref="ConfigureNextGenerationPath"/>
        /// </summary>
        /// <returns></returns>
        public static List<(ElitismChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)> GetAllElitism()
        {
            return new List<(ElitismChoice, Func<ImmutableList<Candidate>, GNAOptions, ImmutableList<Candidate>>)>()
            {
                (ElitismChoice.Default, Elitism),
            };
        }


        /// <summary>
        /// all mutation functions that are used to configure NextGenerationPath HOF inside <see cref="ConfigureNextGenerationPath"/>
        /// </summary>
        /// <returns></returns>
        public static List<(MutationChoice, Func<Candidate, Candidate>)> GetAllMutation()
        {
            return new List<(MutationChoice choice, Func<Candidate, Candidate> func)>()
            {
                (MutationChoice.Default,ApplyPlacementMutationAdapter()),
            };
        }


        /// <summary>
        /// all crossOver functions that are used to configure NextGenerationPath HOF inside <see cref="ConfigureNextGenerationPath"/>
        /// </summary>
        /// <returns></returns>
        public static List<(CrossOverChoice, Func<(Candidate first, Candidate second), (Candidate first, Candidate second)>)> GetAllCrossOver()
        {
            return new List<(CrossOverChoice choice, Func<(Candidate first, Candidate second), (Candidate first, Candidate second)> func)>()
            {
                (CrossOverChoice.Default,ApplyCrossOverAdapter()),
            };
        }


        /// <summary>
        /// all selection functions that are used to configure NextGenerationPath HOF inside <see cref="ConfigureNextGenerationPath"/>
        /// </summary>
        /// <returns></returns>
        public static List<(SelectionChoice, Func<ImmutableList<Candidate>, Candidate>)> GetAllSelection()
        {
            return new List<(SelectionChoice choice, Func<ImmutableList<Candidate>, Candidate>)>()
            {
                (SelectionChoice.Default,RouletteWheelSelectionAdapter()),
                (SelectionChoice.Default,RandomCandidateSelectionAdapter()),
            };
        }


        #endregion


    }









}
