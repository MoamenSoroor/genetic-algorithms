using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Algorithms;
using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalesManProblem.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private readonly Randomizer randomizer;
        private int citiesCount = 32;
        private int iterations = 20;
        private int roundsPerIteration = 100;
        private int populationSize = 300;
        private double crossoverPercentage = 20;
        private double mutationPercentage = 80;
        private double elitismPercentage = 80;
        private List<City> cities;
        private string pathLengthString;
        private string avgPathLengthString;

        // set from window
        private int mapWidth;
        private int mapHeight;

        public MainWindowViewModel(Randomizer randomizer)
        {
            GenerateRandomCities = new RelayCommand(GenerateRandomCitiesMethod);
            RunGNAlgorithm = new AsyncRelayCommand(RunAlgorithmAsync);
            GenerateCircularCities = new RelayCommand(PerformGenerateCircularCities);
            this.randomizer = randomizer;

        }


        public int Iterations { get => iterations; set => SetProperty(ref iterations, value); }
        public int RoundsPerIteration { get => roundsPerIteration; set => SetProperty(ref roundsPerIteration, value); }
        public int PopulationSize { get => populationSize; set => SetProperty(ref populationSize, value); }
        public double CrossoverPercentage { get => crossoverPercentage; set => SetProperty(ref crossoverPercentage, value); }
        public double MutationPercentage { get => mutationPercentage; set => SetProperty(ref mutationPercentage, value); }
        public double ElitismPercentage { get => elitismPercentage; set => SetProperty(ref elitismPercentage, value); }
        public int CitiesCount { get => citiesCount; set => SetProperty(ref citiesCount, value); }
        public List<City> Cities { get => cities; set => SetProperty(ref cities, value); }
        public int MapWidth { get => mapWidth; set => mapWidth = value; }
        public int MapHeight { get => mapHeight; set => mapHeight = value; }
        

        public IRelayCommand GenerateRandomCities { get; }

        public IAsyncRelayCommand RunGNAlgorithm { get; }

        public IRelayCommand GenerateCircularCities { get; }

        public string PathLengthString { get => pathLengthString; set => SetProperty(ref pathLengthString, value); }

        public string AvgPathLengthString { get => avgPathLengthString; set => SetProperty(ref avgPathLengthString, value); }

        private System.Collections.IEnumerable initGNAConfigurations;

        public System.Collections.IEnumerable InitGNAConfigurations { get => initGNAConfigurations; set => SetProperty(ref initGNAConfigurations, value); }

        private System.Collections.IEnumerable fitnessConfigurations;

        public System.Collections.IEnumerable FitnessConfigurations { get => fitnessConfigurations; set => SetProperty(ref fitnessConfigurations, value); }

        private System.Collections.IEnumerable elitismConfigurations;

        public System.Collections.IEnumerable ElitismConfigurations { get => elitismConfigurations; set => SetProperty(ref elitismConfigurations, value); }

        private System.Collections.IEnumerable crossOverConfigurations;

        public System.Collections.IEnumerable CrossOverConfigurations { get => crossOverConfigurations; set => SetProperty(ref crossOverConfigurations, value); }

        private System.Collections.IEnumerable mutationConfigurations;

        public System.Collections.IEnumerable MutationConfigurations { get => mutationConfigurations; set => SetProperty(ref mutationConfigurations, value); }






        private async Task RunAlgorithmAsync()
        {
            var pars = new GNAParameters(iterations,
                populationSize,
                crossoverPercentage / 100D,
                mutationPercentage / 100D,
                elitismPercentage / 100D,
                cities.Select(c => c.Position).ToImmutableList(),
                randomizer,
                roundsPerIteration
                );
            GNAlgorithm algorithm = new GNAlgorithm(pars);
            AvgPathLengthString = $"wait ...";
            PathLengthString = $"";
            var results = await Task.Run(() => algorithm.Execute());
            AvgPathLengthString = $"Avg Path Length: {results.AveragePathLength}";
            PathLengthString = $"Best Path Length: {(int)results.MapPath.PathLength}";

            WeakReferenceMessenger.Default.Send(results.MapPath);

        }


        private void PerformGenerateCircularCities()
        {
            int radius = (int)((double)Math.Min(MapWidth, MapHeight) / 2.2);
            int centerX = MapWidth / 2;
            int centerY = MapHeight / 2;
            double step = (2 * Math.PI) / citiesCount;

            var points = Enumerable.Range(0,citiesCount)
                .Select((v, i) =>
                {
                    int x = centerX + (int)(Math.Cos(i * step)*radius);
                    int y = centerY + (int)(Math.Sin(i * step)*radius);
                    return new City($"{i + 1}", new System.Drawing.Point(x, y));
                }).ToList();
            Cities = points;
            WeakReferenceMessenger.Default.Send(points);
        }

        private void GenerateRandomCitiesMethod()
        {
            var points = GenerateRandomPoints();
            Cities = points;
            WeakReferenceMessenger.Default.Send(points);
        }

        public List<City> GenerateRandomPoints()
        {
            //return randomizer.RandomUniqueSequence(citiesCount, (int)mapWidth)
            //    .Zip(randomizer.RandomUniqueSequence(citiesCount, (int)mapHeight))
            return randomizer.RandomUniqueSequenceWithSpaces(citiesCount, (int)mapWidth,20)
                .Zip(randomizer.RandomUniqueSequenceWithSpaces(citiesCount, (int)mapHeight,20))
                .Select((pair, i) => new City($"{i + 1}", new System.Drawing.Point(pair.First, pair.Second)))
                .ToList();
        }


    }
}
