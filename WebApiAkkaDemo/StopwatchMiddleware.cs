using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebApiAkkaDemo
{
    public class StopwatchMiddleware : OwinMiddleware
    {
        public StopwatchMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await Next.Invoke(context);

            Console.WriteLine("duration: {0}", stopwatch.ElapsedMilliseconds);
        }
    }
}
