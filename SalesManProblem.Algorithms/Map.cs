using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SalesManProblem.Algorithms
{
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


}
