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

            context.Response.OnSendingHeaders(
                state => 
                {
                    float elapsedMilliseconds =
                        (float) stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000f;
                    
                    context.Response.Headers["X-Runtime-Milliseconds"] = elapsedMilliseconds.ToString("N3");
                }, context
            );

            await Next.Invoke(context);
        }
    }
}
