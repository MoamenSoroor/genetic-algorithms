using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExample
{
    public class RandomGenerator
    {


        public static int NextInt(int toInclusive, int toExclusive)
        {
            return RandomNumberGenerator.GetInt32(toInclusive, toExclusive);
        }

        public static int NextInt(int toExclusive)
        {
            return RandomNumberGenerator.GetInt32(toExclusive);
        }

        public static int NextInt()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }

        public static double NextDouble()
        {
            int value = RandomNumberGenerator.GetInt32(int.MaxValue);
            return double.Parse($"0.{value}");
        }

        public static double NextDouble(int min, int max)
        {
            return NextDouble() * (max - min) + min;
        }


        public static IEnumerable<int> RandomUniqueSequence(int count, int max)
        {




            return Enumerable.Range(0, max).OrderBy(v => NextInt()).Take(count);



        }

        public static IEnumerable<int> RandomUniqueSequence(int count, int min, int max)
        {






            return Enumerable.Range(min, max).OrderBy(v => NextInt()).Take(count);


        }

        public static IEnumerable<int> RandomSequence(int count, int max)
        {


            return Enumerable.Repeat(max, count).Select(_ => NextInt(max));

        }

        public static IEnumerable<int> RandomUniqueSequenceWithSpaces(int count, int max, int space)
        {




            return Enumerable.Range(0, max / space)
                .Select(v => v * space)
                .OrderBy(v => NextInt()).Take(count);
        }

        public static IEnumerable<int> RandomSequence(int count, int min, int max)
        {




            return Enumerable.Repeat(max, count).Select(_ => NextInt(min, max));
        }

        public static IEnumerable<int> RandomSequence(int count, int min, int max, int space)
        {





            return Enumerable.Repeat(max, count)
                .Select(_ => NextInt(min, max))
                .Select(value => value - (value % space));
        }



    }

}
