using System;

namespace SalesManProblem.Algorithms.GNA
{
    /// <summary>
    /// a type that ecapsulate the result of one iteration of execution of the algorithm.
    /// </summary>
    public class GNAIterationResult
    {

        /// <summary>
        /// a static function that encapsulates the best Path the returned form one iteration. 
        /// and that result is used to generate the final result <see cref="GNAResult"/>
        /// </summary>
        /// <param name="path">the best path</param>
        /// <param name="elapsed">the execution time of the iteration</param>
        /// <returns>new GNAIterationResult</returns>
        public static GNAIterationResult Create(MapPath path, TimeSpan elapsed)
        {
            return new GNAIterationResult(path, elapsed);
        }

        private GNAIterationResult(MapPath path, TimeSpan elapsed)
        {
            Path = path;
            Elapsed = elapsed;
        }

        /// <summary>
        /// the best path
        /// </summary>
        public MapPath Path { get; }

        /// <summary>
        /// the execution time of the iteration
        /// </summary>
        public TimeSpan Elapsed { get; }
    }


    


}
