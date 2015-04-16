using System;
using Microsoft.Practices.Unity;

namespace UnityDemo
{
    public interface IService 
    {
        void DoSomething();
    }

    public class MyService : IService
    {
        public void DoSomething()
        {
            Console.WriteLine("MyService implements IService");
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            ContainerDemo();
        }

        static void ContainerDemo()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterInstance<IService>(new MyService());

                var myService = container.Resolve<IService>();
                myService.DoSomething();
            }
        }
    }
}
