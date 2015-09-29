using System;
using System.Reactive;
using System.Reactive.Linq;

namespace RxDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var xs = "Hello World".ToObservable();

            xs.Subscribe(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }
}
