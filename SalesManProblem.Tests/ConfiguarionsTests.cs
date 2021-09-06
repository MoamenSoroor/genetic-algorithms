using NUnit.Framework;
using SalesManProblem.Algorithms;
using SalesManProblem.Algorithms.Configurations;
using SalesManProblem.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace SalesManProblem.Tests
{
    public class ConfiguarionsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        public static Map map = Map.Create(new[] {
            City.Create("c1",new Point(20,30)),
            City.Create("c3",new Point(70,20)),
            City.Create("c2",new Point(50,10)),
            City.Create("c4",new Point(20,100)),
        });

        public static List<Point> cities = map.GetAllPositions().ToList();

        public static GNAOptions options = new GNAOptions(1,
                populationSize: 8,
                50 / 100D,
                25 / 100D,
                25 / 100D,
                10
                );


        public static ImmutableList<Candidate> candidates = new List<Candidate>()
        {
            Candidate.Create(MapPath.Create(ImmutableList.Create(cities[0],cities[1],cities[2],cities[3]))),
            Candidate.Create(MapPath.Create(ImmutableList.Create(cities[1],cities[0],cities[3],cities[2]))),
            Candidate.Create(MapPath.Create(ImmutableList.Create(cities[3],cities[2],cities[0],cities[1]))),
            Candidate.Create(MapPath.Create(ImmutableList.Create(cities[2],cities[1],cities[0],cities[3]))),
            
        }.ToImmutableList();




        [Test]
        public void Fitness_Test()
        {
            // prepare
            var list = ImmutableList.Create(Candidate.Create(MapPath.CreateRandom(map)));
            var expected = ImmutableList.Create(Candidate.Create(MapPath.CreateRandom(map), 0));

            // act 
            var result = GNAConfigurations.Fitness(list, options);


            Assert.AreEqual(expected[0].Fitness, result[0].Fitness);

        }


        [Test]
        public void GenerateNewCandidates_Test()
        {
            // prepare
            var list = candidates;

            var expected = new List<Candidate>()
            {
                Candidate.Create(MapPath.Create(ImmutableList.Create(cities[0],cities[1],cities[2],cities[3]))),
                Candidate.Create(MapPath.Create(ImmutableList.Create(cities[1],cities[0],cities[3],cities[2]))),
                Candidate.Create(MapPath.Create(ImmutableList.Create(cities[3],cities[2],cities[0],cities[1]))),
                Candidate.Create(MapPath.Create(ImmutableList.Create(cities[2],cities[1],cities[0],cities[3]))),

            }.ToImmutableList();

            int counter = 0;
            Func<ImmutableList<Candidate>, Candidate> selection = (allCandidates) =>
             {
                 counter = counter < allCandidates.Count ? counter + 1 : counter % allCandidates.Count; 
                 return allCandidates[counter-1];
             };

            // act 
            var result = GNAConfigurations.SelectFromParents(list, options,selection);


            // assert
            Assert.AreEqual(expected.Select(c=> c.Path).ToList(), result.Select(c=> c.Path).ToList());

        }
    }
}