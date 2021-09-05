using Ardalis.GuardClauses;
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
    public class City
    {

        public static readonly City Null = new City("", Point.Empty);


        public static City FromPosition(Point point)
        {
            return new City("", point);
        }


        public static double DistanceFrom(City first, City second)
        {
            return Geometry.Distance(first.position, second.position);
        }

        public City(string name, Point position)
        {
            Guard.Against.Null(name, nameof(name), "name can't be null");
            this.position = position;
            Name = name;
        }

        public City(string name, int x, int y)
        {
            Guard.Against.Null(name, nameof(name), "name can't be null");
            this.position = new Point(x, y);
            Name = name;
        }


        private Point position;


        public string Name { get; }
        public Point Position { get => position; }


        public double DistanceFrom(City another)
        {
            return DistanceFrom(this, another);
        }

        public override bool Equals(object obj)
        {
            return position.Equals(obj);
        }
        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }

    public class Map
    {

        public static Map DefaultMap()
        {
            List<City> cites = new List<City>()
            {
                new City("",new Point(6734 ,1453)),
                new City("",new Point(2233 ,  10)),
                new City("",new Point(5530 ,1424)),
                new City("",new Point( 401 , 841)),
                new City("",new Point(3082 ,1644)),
                new City("",new Point(7608 ,4458)),
                new City("",new Point(7573 ,3716)),
                new City("",new Point(7265 ,1268)),
                new City("",new Point(6898 ,1885)),
                new City("",new Point(1112 ,2049)),
                new City("",new Point(5468 ,2606)),
                new City("",new Point(5989 ,2873)),
                new City("",new Point(4706 ,2674)),
                new City("",new Point(4612 ,2035)),
                new City("",new Point(6347 ,2683)),
                new City("",new Point(6107 , 669)),
                new City("",new Point(7611 ,5184)),
                new City("",new Point(7462 ,3590)),
                new City("",new Point(7732 ,4723)),
                new City("",new Point(5900 ,3561)),
                new City("",new Point(4483 ,3369)),
                new City("",new Point(6101 ,1110)),
                new City("",new Point(5199 ,2182)),
                new City("",new Point(1633 ,2809)),
                new City("",new Point(4307 ,2322)),
                new City("",new Point( 675 ,1006)),
                new City("",new Point(7555 ,4819)),
                new City("",new Point(7541 ,3981)),
                new City("",new Point(3177 , 756)),
                new City("",new Point(7352 ,4506)),
                new City("",new Point(7545 ,2801)),
                new City("",new Point(3245 ,3305)),
                new City("",new Point(6426 ,3173)),
                new City("",new Point(4608 ,1198)),
                new City("",new Point(  23 ,2216)),
                new City("",new Point(7248 ,3779)),
                new City("",new Point(7762 ,4595)),
                new City("",new Point(7392 ,2244)),
                new City("",new Point(3484 ,2829)),
                new City("",new Point(6271 ,2135)),
                new City("",new Point(4985 , 140)),
                new City("",new Point(1916 ,1569)),
                new City("",new Point(7280 ,4899)),
                new City("",new Point(7509 ,3239)),
                new City("",new Point(  10 ,2676)),
                new City("",new Point(6807 ,2993)),
                new City("",new Point(5185 ,3258)),
                new City("",new Point(3023 ,1942)),
            };

            return new Map(cites);
        }


        public static Map Create(IEnumerable<City> cities)
        {
            Guard.Against.NullOrEmpty(cities, nameof(cities), "cities  passed can't be null or empty");
            return new Map(cities);
        }

        public Map(IEnumerable<City> _cities)
        {
            this.cities = _cities.ToHashSet();
        }
        private HashSet<City> cities = new HashSet<City>();

        public IReadOnlySet<City> Cities { get => cities; }

        public City GetCityFromPosition(Point position)
        {
            var isexist = this.cities.TryGetValue(City.FromPosition(position), out City mycity);

            return isexist ? mycity : City.Null;
        }

        public IReadOnlyList<Point> GetAllPositions()
        {
            return this.cities.Select(c => c.Position).ToList();
        }


    }

    //public class MapPathBuilder
    //{
    //    private List<(Point position, double distance)> positions = new List<(Point position, double distance)>();

    //    public MapPathBuilder Add(Point position)
    //    {
    //        if (positions.Count == 0)
    //            positions.Add((position, 0.0));
    //        else
    //        {
    //            var distance = Geometry.Distance(positions.Last().position, position);
    //            positions.Add((position, distance));
    //        }
    //        return this;
    //    }

    //    public MapPathBuilder Insert(int index, Point position)
    //    {
    //        Guard.Against.OutOfRange(index, nameof(index), 0, positions.Count, "insert at index that is out of range");

    //        positions.Insert(index,(position, 0.0));

    //        return this;

    //    }


    //    public MapPathBuilder RemoveAt(int index)
    //    {
    //        Guard.Against.OutOfRange(index, nameof(index), 0, positions.Count, "insert at index that is out of range");
    //        positions.RemoveAt(index);
    //        return this;
    //    }

    //    public MapPathBuilder Remove(Point position)
    //    {
    //        positions.RemoveAll(p=> p.position == position);
    //        return this;
    //    }


    //    public MapPathBuilder Clear()
    //    {
    //        positions.Clear();
    //        return this;
    //    }

    //    public MapPath Build()
    //    {

    //        return new MapPath(positions.ToImmutableList());
    //    }
    //}



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
        public static MapPath Create(Map map)
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
