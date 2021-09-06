using System;

namespace SalesManProblem.Algorithms.GNA
{

    /// <summary>
    /// Type that encapsulates the result of the execution of the algorithm.
    /// </summary>
    public class GNAResult
    {
        private double averagePathLength;
        private MapPath mapPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="averagePathLength"></param>
        /// <param name="path"> the best path with the shortes length</param>
        /// <param name="totalElapsed">total execution time of the GNA</param>
        /// <param name="iterationElapsed">execution time of the iteration that has the best path</param>
        /// <returns>new Genetic Algorithm Results</returns>
        public static GNAResult Create(double averagePathLength, MapPath path, TimeSpan totalElapsed, TimeSpan iterationElapsed)
        {
            return new GNAResult(averagePathLength, path, totalElapsed, iterationElapsed);
        }

        private GNAResult(double averagePathLength, MapPath path, TimeSpan totalElapsed, TimeSpan iterationElapsed)
        {
            this.averagePathLength = averagePathLength;
            this.mapPath = path;
            TotalElapsed = totalElapsed;
            IterationElapsed = iterationElapsed;

        }

        /// <summary>
        /// variable that has the average path length of the whole iterations that executed.
        /// </summary>
        public double AveragePathLength { get => averagePathLength; }


        public double BestPathLength { get => mapPath.PathLength; }

        /// <summary>
        /// the best MapPath
        /// </summary>
        public MapPath MapPath { get => mapPath; }
        
        /// <summary>
        /// the total execution time of the algorithm
        /// </summary>
        public TimeSpan TotalElapsed { get; }
        

        /// <summary>
        /// the winning iteration execution time
        /// </summary>
        public TimeSpan IterationElapsed { get; }
    }


    


}
