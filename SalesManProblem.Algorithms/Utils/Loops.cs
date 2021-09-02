using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.Utils
{
    public static class Loops
    {
        public static TResult AggregateWhile<T, TResult>(Func<T,int,bool> predicate, Func<T, int, TResult> bodyFunc,T element)
        {
            TResult result = default;
            int count = 0;
            while (predicate(element,count))
            {
                result = bodyFunc(element,count);
                count++;
            }
            return result;
        }


        //public static T AggregateWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, T> bodyFunc)
        //{
        //    int count = 0;
        //    while (predicate(element))
        //    {
        //        element = bodyFunc(element);
        //        count++;
        //    }
        //    return element;
        //}



    }
}
