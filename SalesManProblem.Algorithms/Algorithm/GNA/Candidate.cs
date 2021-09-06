namespace SalesManProblem.Algorithms.GNA
{

    /// <summary>
    /// Candidate is Immutable type that represents One Chromosome in Genetic Algorithm. 
    /// It encapsulates MapPath and it's fitness that will be calculated after Fitness 
    /// Process. fitness value by default equals 0.
    /// </summary>
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
        /// create new candidate from a path (shallow copy) with new fitness value.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>new candidate (shallow copy)</returns>
        public static Candidate Create(MapPath path, double fitness)
        {
            return new Candidate(path, fitness);
        }



        private readonly double fitness;

        private readonly MapPath path;

        /// <summary>
        /// fitness value of the candidate
        /// </summary>
        public MapPath Path => path;



        /// <summary>
        /// cities path of the candidate
        /// </summary>
        public double Fitness { get => fitness; }


        private Candidate(MapPath path, double fitness)
        {
            this.path = path;
            this.fitness = fitness;
        }

        public override bool Equals(object obj)
        {
            if(obj is Candidate candidate)
                return path.Equals(candidate) && fitness == candidate.fitness;
            return false;
        }
        public override int GetHashCode()
        {
            return path.GetHashCode();
        }

        

    }


    


}
