using Ardalis.GuardClauses;
using System.Drawing;

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


}
