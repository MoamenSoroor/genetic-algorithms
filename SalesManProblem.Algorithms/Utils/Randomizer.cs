using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.Algorithms.GNA
{
//    public static class Randomizer
//    {
//        public static Random random = new Random();

//        public static double DoubleFromZeroToOne()
//        {
//            return random.NextDouble();
//        }

//        public static int IntLessThan(int max)
//        {
//            return random.Next(max);
//        }


//    }
    public class Randomizer
    {
        public Random random = new Random();

        public double DoubleFromZeroToOne()
        {
            return random.NextDouble();
        }

        public double DoubleLessThan(double min, double max)
        {
            return random.NextDouble() * max;
        }

        public double DoubleBetween(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public int IntLessThan(int max)
        {
            return random.Next(max);
        }

        public int IntBetween(int min, int max)
        {
            return random.Next(min,max);
        }

        public IEnumerable<int> RandomUniqueSequence(int count,int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(max, "max", count, int.MaxValue, "max can't be less than count");

            //var list = new List<int>(count);
            //int i = 0;
            //while (i < count)
            //{
            //    int rand = random.Next(max);
            //    if (!list.Contains(rand))
            //    {
            //        list.Add(rand);
            //        i++;
            //        yield return rand;
            //    }
            //}

            return Enumerable.Range(0, max).OrderBy(v => random.Next()).Take(count);



        }

        public IEnumerable<int> RandomUniqueSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(min, "min", 1, int.MaxValue, "min can't be less than 1");
            Guard.Against.OutOfRange(max, "max", min, int.MaxValue, "max can't be less than min");
            Guard.Against.OutOfRange(count, "count", max-min, int.MaxValue, "count can't be less than max-min");


            //var list = new List<int>(count);
            //int i = 0;
            //while (i < count)
            //{
            //    int rand = random.Next(min,max);
            //    if (!list.Contains(rand))
            //    {
            //        list.Add(rand);
            //        i++;
            //        yield return rand;

            //    }
            //}

            return Enumerable.Range(min, max).OrderBy(v => random.Next()).Take(count);


        }

        public IEnumerable<int> RandomSequence(int count, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");

            //for (int i = 0; i < count; i++)
            //{
            //    yield return random.Next(max);
            //}

            return Enumerable.Repeat(max, count).Select(_ => random.Next(max));

        }

        public IEnumerable<int> RandomUniqueSequenceWithSpaces(int count,int max, int space)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(space, "space", 1, max, "space can't be less than 1 or more than max");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");

            return Enumerable.Range(0, max/space).Select(v=> v*space).OrderBy(v => random.Next()).Take(count);
        }

        public IEnumerable<int> RandomSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            Guard.Against.OutOfRange(min, "min", 0, int.MaxValue, "min can't be less than 0");

            //for (int i = 0; i < count; i++)
            //{
            //    yield return random.Next(min,max);
            //}

            return Enumerable.Repeat(max, count).Select(_ => random.Next(min,max));
        }




    }

}
