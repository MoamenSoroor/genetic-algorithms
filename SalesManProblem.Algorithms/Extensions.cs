using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem
{
    public static class Extensions
    {

        /// <summary>
        /// a Composer Higher order function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return (value) => f2(f1(value));
        }


        /// <summary>
        /// adapter higher order function that is used reduce that parameters of a function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static Func<T1, T3> ReduceParameters<T1, T2, T3>(this Func<T1, T2, T3> f1, T2 t2)
        {
            return (t1) => f1(t1, t2);
        }

        /// <summary>
        /// adapter higher order function that is used reduce that parameters of a function
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="f1"></param>
        /// <param name="t3"></param>
        /// <returns></returns>
        public static Func<T1, T2, T4> ReduceParameters<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f1, T3 t3)
        {
            return (t1, t2) => f1(t1, t2, t3);
        }


    }
}
