using System;
using Quartz;

namespace QuartzDemo
{
    class TickJob : IJob
	{
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("TickJob fired: {0}", DateTime.Now.ToString("G"));
        }
	}
}
