using Microsoft.Owin.Hosting;
using System;

namespace WebApiAkkaDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: "http://*:9000/"))
            {
                Console.WriteLine("Listening on Port 9000");
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}
