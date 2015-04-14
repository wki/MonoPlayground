using System;

namespace OwinDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://localhost:9000"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }        
        }
    }
}
