using System;
using Topshelf;

namespace TopShelfDemo
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            return (int)HostFactory.Run(s =>
                {
                    s.SetServiceName("MyService");
                    s.SetDisplayName("MyService is a Service");
                    s.SetDescription("MyService - a sample service");

                    s.RunAsLocalSystem();
                    s.StartAutomatically();

                    s.Service<MyService>();
                });
        }
    }
}
