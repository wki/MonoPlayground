using System;
using System.Threading.Tasks;
using Orleans;

namespace OrleansDemo
{
    public interface IHello : IGrain
    {
        Task<string> SayHello(string greeting);
    }
}
