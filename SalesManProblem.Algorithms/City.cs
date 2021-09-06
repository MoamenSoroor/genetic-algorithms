using Ardalis.GuardClauses;
using System.Drawing;

namespace SalesManProblem.Algorithms
{
    /// <summary>
    /// City is Immutable type that represent city location on the map
    /// </summary>
    public class City
    {
        public static readonly City Null = new City("X", new Point(-1, -1));
        

        /// <summary>
        /// Create a City
        /// </summary>
        /// <param name="name">name of the city</param>
        /// <param name="position">position of the city</param>
        /// <returns></returns>
        public static City Create(string name, Point position)
        {
            return new City(name, position);
        }

        /// <summary>
        /// Create a City without name (for test purposes)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static City Create(Point point)
        {
            return new City("_", point);
        }


        public static double DistanceFrom(City first, City second)
        {
            return Geometry.Distance(first.position, second.position);
        }

        private City(string name, Point position)
        {
            //Guard.Against.Null(name, nameof(name), "name can't be null");
            this.position = position;
            this.name = name;
        }

        private City(string name, int x, int y)
        {
            //Guard.Against.Null(name, nameof(name), "name can't be null");
            this.position = new Point(x, y);
            this.name = name;
        }


        private readonly Point position;
        private readonly string name;


        /// <summary>
        /// name of the city
        /// </summary>
        public string Name => name;


        /// <summary>
        /// position of  the city
        /// </summary>
        public Point Position => position; 


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


}
