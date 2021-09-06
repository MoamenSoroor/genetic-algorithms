using SalesManProblem.Algorithms.Algorithms.GNA;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms
{

    /// <summary>
    /// MapPath is Immutable Type that represents a valid path on the map. 
    /// It encapsulates ImmutableList of System.Drawing.Point that each one 
    /// of that points represent City Location (x,y)
    /// </summary>
    public class MapPath
    {



        /// <summary>
        /// this method is used to calculate the path length
        /// </summary>
        /// <returns>returns the length of the MapPath</returns>
        public static double CalculatePathLength(MapPath path)
        {

            var len = Enumerable.Zip(path.positions, path.positions.Skip(1).Append(path.positions.First()))
                .Sum((pos) => Geometry.Distance(pos.First, pos.Second));
            return len;
        }


        /// <summary>
        /// create map path from another one (shallow copy)
        /// </summary>
        /// <param name="path"></param>
        /// <returns>new path</returns>
        public static MapPath Create(MapPath path)
        {
            return new MapPath(path);
        }

        /// <summary>
        /// create random path from the cities on the map (deep copy)
        /// </summary>
        /// <param name="map">the map that has the city positions</param>
        /// <returns>new path with random positions</returns>
        public static MapPath CreateRandom(Map map)
        {
            var positions = map.GetAllPositions();
            var path = new MapPath(RandomGenerator.RandomUniqueSequence(positions.Count, positions.Count)
                .Select(index => positions[index]).ToImmutableList());

            return path;
        }

        /// <summary>
        /// create path from city positions directly without map (deep copy)
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static MapPath Create(ImmutableList<Point> positions)
        {
            return new MapPath(positions);
        }

        /// <summary>
        /// create new path after applying swaps on the city positions (deep copy).
        /// It used by Crossover Process
        /// </summary>
        /// <param name="path">the old MapPath that will be used to generate new one</param>
        /// <param name="swaps">swaps that will be applied to the new MapPath</param>
        /// <returns>new MapPath from the previous</returns>
        public static MapPath Create(MapPath path, ImmutableList<(int Index, int NewIndex)> swaps)
        {
            // using list because ImmutableList indexer is O(log n) not O(1)
            var list = new List<Point>(path.Positions);

            swaps.ForEach(pair =>
            {
                var temp = list[pair.Index];
                list[pair.Index] = list[pair.NewIndex];
                list[pair.NewIndex] = temp;
            });

            return MapPath.Create(list.ToImmutableList());
        }

        /// <summary>
        /// create new map path after applying swap on two positions (deep copy).
        /// It used by Crossover Process
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <param name="newIndex"></param>
        /// <returns></returns>
        public static MapPath Create(MapPath path, int index, int newIndex)
        {
            var list = new List<Point>(path.Positions);
            var temp = list[index];
            list[index] = list[newIndex];
            list[newIndex] = temp;
            return MapPath.Create(list.ToImmutableList());
        }



        private MapPath(MapPath path)
        {
            this.positions = path.positions;
            pathLength = path.pathLength;
        }

        private MapPath(ImmutableList<Point> positions)
        {
            this.positions = positions;
            this.pathLength = CalculatePathLength(this);
        }

        private ImmutableList<Point> positions;
        private double pathLength;

        /// <summary>
        /// path length that is calculated on the constructor of the MapPath.
        /// </summary>
        public double PathLength { get => pathLength; }

        /// <summary>
        /// the Number of Cities on the Path
        /// </summary>
        public int CityCount { get => positions.Count; }

        /// <summary>
        /// The Cities Positions on the Path
        /// </summary>
        public ImmutableList<Point> Positions { get => positions; }



        public override bool Equals(object obj)
        {
            if(obj is MapPath path)
            {
                return Enumerable.SequenceEqual(path.Positions, this.positions);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return positions.GetHashCode();
        }

    }


}
