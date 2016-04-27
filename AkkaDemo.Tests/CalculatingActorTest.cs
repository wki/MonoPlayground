using AkkaDemo;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using System;

namespace AkkaDemo.Tests
{
    [TestFixture]
    public class Test : TestKit
    {
        //TestActorRef<CalculatingActor> calculatingActor;
        IActorRef calculatingActor;

        [SetUp]
        public void SetUp()
        {
//            this.calculatingActor = ActorOfAsTestActorRef<CalculatingActor>(
//                Props.Create<CalculatingActor>()
//            );
            this.calculatingActor = Sys.ActorOf(
                Props.Create<CalculatingActor>()
            );

        }

        [Test]
        public void AddToValue_WithValue_HasNoResult()
        {
            // Act
            calculatingActor.Tell(new CalculatingActor.AddMessage(5));

            // Assert
            ExpectNoMsg();
        }

        [Test]
        public void Value_Initially_Is0()
        {
            // Act
            calculatingActor.Tell(new CalculatingActor.AskValue());

            // Assert
            var answer = ExpectMsg<int>();
            Assert.AreEqual(0, answer);
        }

        [Test]
        public void Value_AfterAdding5_Is5()
        {
            // Act
            calculatingActor.Tell(new CalculatingActor.AddMessage(5));
            calculatingActor.Tell(new CalculatingActor.AskValue());

            // Assert
            var answer = ExpectMsg<int>();
            Assert.AreEqual(5, answer);
        }
    }
}
