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
        TestActorRef<CalculatingActor> calculatingActor;

        [SetUp]
        public void SetUp()
        {
            this.calculatingActor = ActorOfAsTestActorRef<CalculatingActor>(
                Props.Create(() => new CalculatingActor())
            );
        }

        [Test]
        public void Value_Initially_Is0()
        {
            // Assert
            Assert.AreEqual(0, calculatingActor.UnderlyingActor.Value);
        }

        [Test]
        public void Value_AfterAdding5_Is5()
        {
            // Act
            calculatingActor.Tell(new CalculatingActor.AddMessage(5));

            // Assert
            Assert.AreEqual(5, calculatingActor.UnderlyingActor.Value);
        }

        [Test]
        public void ReturnValue_SendsInt()
        {
            // Act
            calculatingActor.Tell(new CalculatingActor.AskValue());

            // Assert
            var answer = ExpectMsg<int>();
            Assert.AreEqual(0, answer);
        }
    }
}
