using System;
using Quartz;
using Quartz.Impl;
using Quartz.API;

namespace QuartzDemo
{
    public static class MainClass
    {
        private const int RUNTIME = 120;
        private static IScheduler scheduler;

        public static void Main(string[] args)
        {
            Console.WriteLine("Quartz is running. Program automatically stops after {0} seconds!", RUNTIME);
            Console.WriteLine("Administration possible via http://localhost:9001");
            Console.WriteLine("See https://github.com/ryudice/QuartzNetAPI for description");

            BuildScheduler();
            AddTickJob();

            QuartzAPI.Configure(builder =>
                {
                    builder.UseScheduler(scheduler);
                    builder.EnableCors();
                });
            QuartzAPI.Start("http://localhost:9001/");
        }

        static void BuildScheduler()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
        }

        static void AddTickJob()
        {
            var job = JobBuilder.Create<TickJob>()
                .WithDescription("a regularly running ticker")
                .Build();


            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
