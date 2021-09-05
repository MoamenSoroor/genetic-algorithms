using System;

namespace SalesManProblem.Algorithms.GNA
{
    public class GNAResult
    {
        private int averagePathLength;
        //private readonly int totalPathLength;
        //private IImmutableList<Point> positions;
        private MapPath mapPath;

        //public GNAResult(int averagePathLength, int totalPathLength, IImmutableList<Point> positions)
        //{
        //    this.averagePathLength = averagePathLength;
        //    //this.totalPathLength = totalPathLength;
        //    //this.positions = positions;

        //}

        public GNAResult(int averagePathLength, MapPath path, TimeSpan totalElapsed, TimeSpan iterationElapsed)
        {
            this.averagePathLength = averagePathLength;
            this.mapPath = path;
            TotalElapsed = totalElapsed;
            IterationElapsed = iterationElapsed;
            //this.totalPathLength = (int)path.PathLength;
            //this.positions = path.Positions;
        }

        public int AveragePathLength { get => averagePathLength; }
        //public int TotalPathLength { get => totalPathLength; }
        //public IImmutableList<Point> Positions { get => positions; }
        public MapPath MapPath { get => mapPath; }
        public TimeSpan TotalElapsed { get; }
        public TimeSpan IterationElapsed { get; }
    }


    


}
