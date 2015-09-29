using System;
using Orleans;

namespace OrleansDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            GrainClient.Initialize("DevTestClientConfiguration.xml");

            // not found! does not match documentation
            // var friend = GrainFactory.GrainFactory.
        }
    }
}
