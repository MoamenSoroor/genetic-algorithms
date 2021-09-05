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


    


}
