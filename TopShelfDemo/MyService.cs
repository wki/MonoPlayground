using System;
using Topshelf;

namespace TopShelfDemo
{
    public class MyService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Console.WriteLine("Starting MyService");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("Stopping MyService");
            return true;
        }
    }
}

