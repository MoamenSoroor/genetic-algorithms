namespace SalesManProblem.Algorithms.GNA
{
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
