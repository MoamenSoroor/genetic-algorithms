using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;

namespace SalesManProblem.Algorithms.Algorithms.GNA
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

        public static double NextDouble(double max)
        {
            return NextDouble() * max;
        }


        public static double NextDouble(int max)
        {
            return NextDouble() * max;
        }

        public static IEnumerable<int> RandomUniqueSequence(int count, int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(max, "max", count, int.MaxValue, "max can't be less than count");

            return Enumerable.Range(0, max).OrderBy(v => NextInt()).Take(count);



        }

        public static IEnumerable<int> RandomUniqueSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(min, "min", 1, int.MaxValue, "min can't be less than 1");
            Guard.Against.OutOfRange(max, "max", min, int.MaxValue, "max can't be less than min");
            Guard.Against.OutOfRange(count, "count", max - min, int.MaxValue, "count can't be less than max-min");

            return Enumerable.Range(min, max).OrderBy(v => NextInt()).Take(count);


        }

        public static IEnumerable<int> RandomSequence(int count, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            return Enumerable.Repeat(max, count).Select(_ => NextInt(max));

        }

        public static IEnumerable<int> RandomUniqueSequenceWithSpaces(int count, int max, int space)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(space, "space", 1, max, "space can't be less than 1 or more than max");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");

            return Enumerable.Range(0, max / space)
                .Select(v => v * space)
                .OrderBy(v => NextInt()).Take(count);
        }

        public static IEnumerable<int> RandomSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            Guard.Against.OutOfRange(min, "min", 0, int.MaxValue, "min can't be less than 0");

            return Enumerable.Repeat(max, count).Select(_ => NextInt(min, max));
        }

        public static IEnumerable<int> RandomSequence(int count, int min, int max, int space)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            Guard.Against.OutOfRange(min, "min", 0, int.MaxValue, "min can't be less than 0");
            Guard.Against.OutOfRange(space, "space", 0, int.MaxValue, "space can't be less than 0");

            return Enumerable.Repeat(max, count)
                .Select(_ => NextInt(min, max))
                .Select(value => value - (value % space));
        }



    }


}
