using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.Utils
{
    public static class Extensions
    {
        //public static TResult AggregateWhile<T, TResult>(Func<T,int,bool> predicate, Func<T, int, TResult> bodyFunc,T element)
        //{
        //    TResult result = default;
        //    int count = 0;
        //    while (predicate(element,count))
        //    {
        //        result = bodyFunc(element,count);
        //        count++;
        //    }
        //    return result;
        //}


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


        public static Func<T1,T3> Compose<T1,T2,T3>(this Func<T1,T2> f1,Func<T2,T3> f2)
        {
            return (value) => f2(f1(value));
        }

        public static Func<T1, T3> ReduceParameters<T1, T2, T3>(this Func<T1, T2,T3> f1, T2 t2)
        {
            return (T1 t1) => f1(t1,t2);
        }

        public static Func<T1,T2, T4> ReduceParameters<T1, T2,T3, T4>(this Func<T1, T2, T3,T4> f1, T3 t3)
        {
            return (T1 t1,T2 t2) => f1(t1,t2,t3);
        }


    }
}
