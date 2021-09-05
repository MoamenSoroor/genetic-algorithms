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

    public class MapPath
    {

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
        /// <param name="randomizer">the randomizer that is used to generate random path</param>
        /// <returns>new path with random positions</returns>
        public static MapPath CreateRandom(Map map)
        {
            var positions = map.GetAllPositions();
            var path = new MapPath(RandomGenerator.RandomUniqueSequence(positions.Count, positions.Count)
                .Select(index => positions[index]).ToImmutableList());

            return path;
        }

        /// <summary>
        /// create path from city positions directly with out map (deep copy)
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static MapPath Create(ImmutableList<Point> positions)
        {
            return new MapPath(positions);
        }

        /// <summary>
        /// create new path after applying swaps on the city positions (deep copy)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="swaps"></param>
        /// <returns></returns>
        public static MapPath Create(MapPath path, ImmutableList<(int Index, int NewIndex)> swaps)
        {
            var list = new List<Point>(path.Positions);

            swaps.ForEach(pair =>
            {
                var temp = list[pair.Index];
                list[pair.Index] = list[pair.NewIndex];
                list[pair.NewIndex] = temp;
            });

            return MapPath.Create(MapPath.Create(list.ToImmutableList()));
        }

        /// <summary>
        /// create new map path after applying swap on two positions (deep copy)
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
            return MapPath.Create(MapPath.Create(list.ToImmutableList()));
        }



        private MapPath(MapPath path)
        {
            this.positions = path.positions;
            pathLength = path.pathLength;
        }

        private MapPath(ImmutableList<Point> positions)
        {
            this.positions = positions;
            this.pathLength = CalculatePathLength();
        }

        private ImmutableList<Point> positions;
        private double pathLength;

        public double PathLength { get => pathLength; }
        public int CityCount { get => positions.Count; }
        public ImmutableList<Point> Positions { get => positions; }

        private double CalculatePathLength()
        {

            var len = Enumerable.Zip(positions, positions.Skip(1).Append(positions.First()))
                .Sum((pos) => Geometry.Distance(pos.First, pos.Second));
            return len;
        }

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

        //public MapPath Replace(int index, Point position)
        //{

        //    var newPositions = positions.Select((v, i) => i == index ? position : v).ToImmutableList();
        //    return new MapPath(newPositions);
        //}
        //public MapPath Swap(int index1, int index2)
        //{
        //    var point1 = positions[index1];
        //    var point2 = positions[index2];

        //    var newPositions = positions.Select((value, i) => 
        //        i == index1 ? 
        //            point2 : i == index2 ?
        //                point1 : value).ToImmutableList();
        //    return new MapPath(newPositions);
        //}

    }


}
