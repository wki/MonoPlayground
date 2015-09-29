using System;
using Orleans;
using System.Threading.Tasks;

namespace OrleansDemo
{
    public class HelloGrain : Grain, IHello
    {
        public Task<string> SayHello(string greeting)
        {
            return Task.FromResult(String.Format("You said: '{0}', I say: Hello!", greeting));
        }
    }
}
