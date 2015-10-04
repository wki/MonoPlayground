using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;
// using System.Threading;
using System.Threading;

namespace RxDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            HelloWorld();
            // Test();
            // Grouping();
            // FloatingWindow();

            Console.WriteLine("Done with all.");
            Console.ReadLine();
        }

        // observe characters of a string
        public static void HelloWorld()
        {
            var xs = "Hello World".ToObservable();

            xs.Subscribe(x => { Console.WriteLine(x); Thread.Sleep(TimeSpan.FromMilliseconds(500)); });
        }

        public static void ExperimentalStuff()
        {
        }

        // observe one event occuring
        public static void Test()
        {
            var s = new Subject<int>();
            using (s.Subscribe(i => Console.WriteLine("Next: {0}", i)))
            {
                s.OnNext(42);
            }
        }

        // group input lines by length
        public static void Grouping()
        {
            var source = GetInput().ToObservable(Scheduler.Default);
            var result = source.GroupBy(s => s.Length);

            result.ForEachAsync(g =>
                {
                    Console.WriteLine("New group with Length: {0}", g.Key);

                    g.Subscribe(x => Console.WriteLine("  {0} member of {1}", x, g.Key));
                }
            ).Wait();
        }

        // pack together 3 things into a non overlapping window
        public static void NonOverlappingWindow()
        {
            var source = GetInput().ToObservable(Scheduler.Default);
            var result = source.Window(3);

            result.ForEachAsync(w =>
                {
                    Console.WriteLine("New window created");
                    w.Subscribe(Console.WriteLine);
                }
            ).Wait();
        }

        // pack together 3 things into a floating window
        public static void FloatingWindow()
        {
            var source = GetInput().ToObservable(Scheduler.Default);
            var result = source.Window(3,1);

            var i = 0;
            result.ForEachAsync(w =>
                {
                    var j = i++;
                    Console.WriteLine("New window {0} created", j);
                    w.Subscribe(x => Console.WriteLine("  {0} in {1}", x, j));
                }
            ).Wait();
        }

        // generate lines of text
        public static IEnumerable<string> GetInput()
        {
            while (true)
                yield return Console.ReadLine();
        }
    }
}
