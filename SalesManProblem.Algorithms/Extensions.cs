using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms
{
    
    public static class LinqExtensions
    {

        public static void DoWhile<T>(Func<T,int,bool> predicate,Action<int> body)
        {
            //int round = 0;
            //do
            //{
            //    body(round);
            //} while (predicate());
        }
    }
}
