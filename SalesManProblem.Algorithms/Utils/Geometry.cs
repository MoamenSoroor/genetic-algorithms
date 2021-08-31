using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms
{
    
    public static class Geometry
    {
        public static double Distance(Point first, Point second)
        {
            var result= Math.Abs(Math.Sqrt(Math.Pow((first.X - second.X),2) + Math.Pow((first.Y - second.Y), 2)));

            return result;
        }
    }
}
