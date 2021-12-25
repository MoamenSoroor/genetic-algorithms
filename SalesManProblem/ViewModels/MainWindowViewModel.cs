using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Algorithms;
using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.Configurations;
using SalesManProblem.Algorithms.GNA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SalesManProblem.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private int citiesCount = 32;
        private int iterations = 2;
        private int numberOfGenerationsPerIeration = 500;
        private int populationSize = 1500;
        private double crossoverPercentage = 50;
        private double mutationPercentage = 25;
        private double elitismPercentage = 40;
        private List<City> cities;
        private string pathLengthString;
        private string avgPathLengthString;

        // set from Canvas
        private int mapWidth;
        private int mapHeight;

        public MainWindowViewModel()
        {
            GenerateRandomCities = new RelayCommand(GenerateRandomCitiesMethod);
            RunGNAlgorithm = new AsyncRelayCommand(RunAlgorithmAsync);
            GenerateCircularCities = new RelayCommand(PerformGenerateCircularCities);

            LoadedCommand = new RelayCommand(WindowLoadedHandler);
        }

        private void WindowLoadedHandler()
        {
            GenerateRandomCitiesMethod();
        }

        public int Iterations { get => iterations; set => SetProperty(ref iterations, value); }
        public int NumberOfGenerationsPerIeration { get => numberOfGenerationsPerIeration; set => SetProperty(ref numberOfGenerationsPerIeration, value); }
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


        public IRelayCommand LoadedCommand { get; }

        public string PathLengthString { get => pathLengthString; set => SetProperty(ref pathLengthString, value); }

        public string AvgPathLengthString { get => avgPathLengthString; set => SetProperty(ref avgPathLengthString, value); }


        private string logs = "reports here";

        public string Logs { get => logs; set => SetProperty(ref logs, value); }


        public List<InitGenerationChoice> InitGNAConfigurations { get => Enum.GetValues(typeof(InitGenerationChoice)).Cast<InitGenerationChoice>().ToList(); }


        public List<FitnessChoice>  FitnessConfigurations { get => Enum.GetValues(typeof(FitnessChoice)).Cast<FitnessChoice>().ToList(); }


        public List<ElitismChoice> ElitismConfigurations { get => Enum.GetValues(typeof(ElitismChoice)).Cast<ElitismChoice>().ToList(); }


        public List<CrossOverChoice> CrossOverConfigurations { get => Enum.GetValues(typeof(CrossOverChoice)).Cast<CrossOverChoice>().ToList(); }


        public List<MutationChoice> MutationConfigurations { get => Enum.GetValues(typeof(MutationChoice)).Cast<MutationChoice>().ToList(); }
        
        public List<SelectionChoice> SelectionConfigurations { get => Enum.GetValues(typeof(SelectionChoice)).Cast<SelectionChoice>().ToList(); }

        public InitGenerationChoice SelectedInitGenerationChoice { get; set; } = InitGenerationChoice.Default;
        public FitnessChoice SelectedFitnessChoice { get; set; } = FitnessChoice.Default;
        public SelectionChoice SelectedSelectionChoice { get; set; } = SelectionChoice.Default;
        public ElitismChoice SelectedElitismChoice { get; set; } = ElitismChoice.Default;
        public CrossOverChoice SelectedCrossOverChoice { get; set; } = CrossOverChoice.Default;
        public MutationChoice SelectedMutaionChoice { get; set; } = MutationChoice.Default;


        private async Task RunAlgorithmAsync()
        {
            try
            {
                var options = new GNAOptions(iterations,
                        populationSize,
                        crossoverPercentage / 100D,
                        mutationPercentage / 100D,
                        elitismPercentage / 100D,
                        numberOfGenerationsPerIeration
                        );
                var choices = new GNAChoices(SelectedFitnessChoice, SelectedElitismChoice, SelectedSelectionChoice, SelectedCrossOverChoice, SelectedMutaionChoice);
                GNAlgorithm algorithm = new GNAlgorithm(choices, options);
                AvgPathLengthString = $"wait ...";
                PathLengthString = $"";
                var results = await Task.Run(() => algorithm.Execute(Map.Create(Cities)));
                AvgPathLengthString = $"Avg Path Length: {results.AveragePathLength}";
                PathLengthString = $"Best Path Length: {(int)results.MapPath.PathLength}";


                LogInfo(results);

                WeakReferenceMessenger.Default.Send(results.MapPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        public void LogInfo(GNAResult results)
        {
            string logstr = $@"Algorihm Results.
--------------------------------------------------------
Best Path Length-----------:{results.MapPath.PathLength}
Average Path Length--------:{results.AveragePathLength}
Best Execution Time--------:{results.IterationElapsed}
Total Execution Time-------:{results.TotalElapsed}
Path-----------------------:
{string.Join($"        {Environment.NewLine}", results.MapPath.Positions.Select(p => $"{Cities.FirstOrDefault(c => c.Position == p).Name ?? "C"}:({p.X} , {p.Y})"))}

";
            Logs = logstr;
        }

        private void PerformGenerateCircularCities()
        {
            int radius = (int)((double)Math.Min(MapWidth, MapHeight) / 2.2);
            int centerX = MapWidth / 2;
            int centerY = MapHeight / 2;
            double step = (2 * Math.PI) / citiesCount;

            var points = Enumerable.Range(0, citiesCount)
                .Select((v, i) =>
                {
                    int x = centerX + (int)(Math.Cos(i * step) * radius);
                    int y = centerY + (int)(Math.Sin(i * step) * radius);
                    return City.Create($"{i + 1}", new System.Drawing.Point(x, y));
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
            return RandomGenerator.RandomSequence(citiesCount, 10, mapWidth, 20)
                .Zip(RandomGenerator.RandomSequence(citiesCount, 10, mapHeight, 20))
                .Select((pair, i) => City.Create($"{i + 1}", new System.Drawing.Point(pair.First, pair.Second)))
                .ToList();
        }




    }
}
