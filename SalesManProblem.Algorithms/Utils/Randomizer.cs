using Ardalis.GuardClauses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.Algorithms.GNA
{

    #region ProducerConsumerRandomizerGenerator

    //public class ProducerConsumerRandomizerGenerator : IDisposable
    //{
    //    private static readonly int MaxCount = Environment.ProcessorCount * 3;

    //    // lock for the producer function
    //    private static readonly object producerLock = new object();



    //    //private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
    //    private readonly CountdownEvent cde = new CountdownEvent(MaxCount);
    //    private readonly BlockingCollection<double> queue = new BlockingCollection<double>();
    //    private readonly Random random = new Random();

    //    public ProducerConsumerRandomizerGenerator()
    //    {
            
    //        Produce();
    //    }

    //    private void Produce()
    //    {
    //        Task.Run(() =>
    //        {
    //            lock (producerLock)
    //            {
    //                while (true)
    //                {
                        
    //                    cde.Wait();
    //                    foreach (var item in Enumerable.Range(0, MaxCount))
    //                        queue.Add(random.NextDouble());
    //                    cde.Reset();

    //                }
    //            }
    //        });

            

    //        //var disposable = Observable..Create(observer =>
    //        //{
    //        //    observer.OnNext();
    //        //});

    //    }



    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public virtual int Next()
    //    {
    //        return (int)(NextDouble() * int.MaxValue);

    //    }
    //    public virtual int Next(int maxValue)
    //    {
    //        return (int)(NextDouble() * maxValue);
    //    }
    //    public virtual int Next(int minValue, int maxValue)
    //    {
    //        return (int)(NextDouble() * (maxValue - minValue) + minValue);
    //    }

    //    public virtual double NextDouble()
    //    {
    //        cde.Signal();
    //        return queue.Take();

    //    }



    //}

    #endregion



    public class Randomizer
    {
        private static readonly ThreadLocal<Random> localRandom = 
            new ThreadLocal<Random>(()=> new Random(Guid.NewGuid().GetHashCode()));

        public Randomizer()
        {
            //random = localRandom.Value;
        }
        //public Random random;

        public double DoubleFromZeroToOne()
        {
            return localRandom.Value.NextDouble();
        }

        public double DoubleLessThan(double min, double max)
        {
            return localRandom.Value.NextDouble() * max;
        }

        public double DoubleBetween(double min, double max)
        {
            return localRandom.Value.NextDouble() * (max - min) + min;
        }

        public int IntLessThan(int max)
        {
            return localRandom.Value.Next(max);
        }

        public int IntBetween(int min, int max)
        {
            return localRandom.Value.Next(min, max);
        }

        public IEnumerable<int> RandomUniqueSequence(int count, int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(max, "max", count, int.MaxValue, "max can't be less than count");

            //var list = new List<int>(count);
            //int i = 0;
            //while (i < count)
            //{
            //    int rand = localRandom.Value.Next(max);
            //    if (!list.Contains(rand))
            //    {
            //        list.Add(rand);
            //        i++;
            //        yield return rand;
            //    }
            //}

            return Enumerable.Range(0, max).OrderBy(v => localRandom.Value.Next()).Take(count);



        }

        public IEnumerable<int> RandomUniqueSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");
            Guard.Against.OutOfRange(min, "min", 1, int.MaxValue, "min can't be less than 1");
            Guard.Against.OutOfRange(max, "max", min, int.MaxValue, "max can't be less than min");
            Guard.Against.OutOfRange(count, "count", max - min, int.MaxValue, "count can't be less than max-min");


            //var list = new List<int>(count);
            //int i = 0;
            //while (i < count)
            //{
            //    int rand = localRandom.Value.Next(min,max);
            //    if (!list.Contains(rand))
            //    {
            //        list.Add(rand);
            //        i++;
            //        yield return rand;

            //    }
            //}

            return Enumerable.Range(min, max).OrderBy(v => localRandom.Value.Next()).Take(count);


        }

        public IEnumerable<int> RandomSequence(int count, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");

            //for (int i = 0; i < count; i++)
            //{
            //    yield return localRandom.Value.Next(max);
            //}

            return Enumerable.Repeat(max, count).Select(_ => localRandom.Value.Next(max));

        }

        public IEnumerable<int> RandomUniqueSequenceWithSpaces(int count, int max, int space)
        {
            Guard.Against.OutOfRange(count, "count", 1, int.MaxValue, "count can't be less than 1");
            Guard.Against.OutOfRange(space, "space", 1, max, "space can't be less than 1 or more than max");
            Guard.Against.OutOfRange(max, "max", 1, int.MaxValue, "max can't be less than 1");

            return Enumerable.Range(0, max / space).Select(v => v * space).OrderBy(v => localRandom.Value.Next()).Take(count);
        }

        public IEnumerable<int> RandomSequence(int count, int min, int max)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            Guard.Against.OutOfRange(min, "min", 0, int.MaxValue, "min can't be less than 0");

            //for (int i = 0; i < count; i++)
            //{
            //    yield return localRandom.Value.Next(min,max);
            //}

            return Enumerable.Repeat(max, count).Select(_ => localRandom.Value.Next(min, max));
        }


        public IEnumerable<int> RandomSequence(int count, int min, int max, int space)
        {
            Guard.Against.OutOfRange(count, "count", 0, int.MaxValue, "count can't be less than 0");
            Guard.Against.OutOfRange(max, "max", 0, int.MaxValue, "max can't be less than 0");
            Guard.Against.OutOfRange(min, "min", 0, int.MaxValue, "min can't be less than 0");
            Guard.Against.OutOfRange(space, "space", 0, int.MaxValue, "space can't be less than 0");

            //for (int i = 0; i < count; i++)
            //{
            //    yield return localRandom.Value.Next(min,max);
            //}
            return Enumerable.Repeat(max, count)
                .Select(_ => localRandom.Value.Next(min, max))
                .Select(value => value - (value % space));
        }


    }

}
