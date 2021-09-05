using System;

namespace SalesManProblem.Algorithms.GNA
{
    public class GNAIterationResult
    {


        public GNAIterationResult(MapPath path, TimeSpan elapsed)
        {
            Path = path;
            Elapsed = elapsed;
        }

        //public int TotalPathLength { get => (int)Path.PathLength; }
        //public IImmutableList<Point> Positions { get => Path.Positions; }

        public MapPath Path { get; }
        public TimeSpan Elapsed { get; }
    }


    


}
