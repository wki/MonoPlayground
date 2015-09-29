using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var xs = "Hello World".ToObservable();

            xs.Subscribe(x => Console.WriteLine(x));

            Test();

            Console.ReadLine();
        }

        public static void Test()
        {
            var s = new Subject<int>();
            using (s.Subscribe(i => Console.WriteLine("Next: {0}", i)))
            {
                s.OnNext(42);
            }
        }
    }
}
