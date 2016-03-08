using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace UnityDemo
{
    // the Interface we are registering
    public interface IService 
    {
        void DoSomething();
        int GetSomething();
    }

    // the only implementation of the above interface
    public class MyService : IService
    {
        public void DoSomething()
        {
            Console.WriteLine("MyService implements IService");
        }

        public int GetSomething()
        {
            return 42;
        }
    }

    // das Verhalten, das wir via Interception anwenden
    public class LogBehavior: IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Console.WriteLine("behavior interceptor begins. Method: {0}", input.MethodBase.Name);

            var result = getNext()(input, getNext);

            Console.WriteLine("behavior interceptor done");

            return result;
        }

        public System.Collections.Generic.IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }

    // der Handler für die Policy
    public class LoggingCallHandler : ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Console.WriteLine("call handler interceptor begins. Method: {0}", input.MethodBase.Name);

            var result = getNext()(input, getNext);

            Console.WriteLine("call handler interceptor done");

            return result;
        }

        public int Order { get; set; }
    }

    // commandline app
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
                // instance registration
                // container.RegisterInstance<IService>(new MyService());

                // type registration
                // container.RegisterType<IService, MyService>();

                // type registration with interception
                container.AddNewExtension<Interception>();
                container.RegisterType<IService, MyService>(
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LogBehavior>()
                );

                // type registration with policy for Do* methods
                // container.AddNewExtension<Interception>();
                // container.RegisterType<IService, MyService>(
                //     new InterceptionBehavior<PolicyInjectionBehavior>(),
                //     new Interceptor<InterfaceInterceptor>()
                // );
                // 
                // container.Configure<Interception>()
                //     .AddPolicy("logging")
                //     .AddMatchingRule<AssemblyMatchingRule>(
                //         new InjectionConstructor(
                //             new InjectionParameter("UnityDemo")))
                //     .AddMatchingRule<MemberNameMatchingRule>(
                //         new InjectionConstructor(new [] { "Do*" }, true))
                //     .AddCallHandler<LoggingCallHandler>(
                //         new ContainerControlledLifetimeManager(),
                //         new InjectionConstructor(),
                //         new InjectionProperty("Order", 1));

                // Ausprobieren:
                var myService = container.Resolve<IService>();
                myService.DoSomething();
                Console.WriteLine("Get: {0}", myService.GetSomething());
            }
        }
    }
}
