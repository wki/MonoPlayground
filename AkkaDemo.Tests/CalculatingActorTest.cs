using AkkaDemo;
using Akka.Actor;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using System;

namespace AkkaDemo.Tests
{
    [TestFixture]
    public class Test : TestKit
    {
        IActorRef calculatingActor;

        [SetUp]
        public void SetUp()
        {
            CreateTestActor("test");
            this.calculatingActor = ActorOf<CalculatingActor>();
        }

        [TearDown]
        public async void TearDown()
        {
            Shutdown();
        }

//        [Test]
//        public void Ask_Always_ReturnsTask()
//        {
//            // Act
//            var result = calculatingActor.Ask<int>(new CalculatingActor.AskValue(), TimeSpan.FromSeconds(1));
//            Console.WriteLine("Got Result: {0}", result);
//
//            // Assert
//            Assert.IsNotNull(result);
//        }

        [Test]
        public async void Initial_Value_IsZero()
        {
            // Act
            var result = calculatingActor.Ask<int>(new CalculatingActor.AskValue(), TimeSpan.FromSeconds(1));
            Console.WriteLine("Task Status: {0}", result.Status);
            result.Wait();
            Console.WriteLine("Task Status: {0}", result.Status);

            // Assert
            Assert.AreEqual(0, await result);
        }
    }
}
