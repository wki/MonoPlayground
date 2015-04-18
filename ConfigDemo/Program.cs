using System;
using System.Diagnostics;

namespace ConfigDemo
{
    class MainClass
    {
        static readonly TraceSwitch traceLevel = new TraceSwitch("TraceLevel", "TraceLevel");
        
        public static void Main(string[] args)
        {
            Console.WriteLine("TraceLevel = {0}", traceLevel.Level);
        }
    }
}
