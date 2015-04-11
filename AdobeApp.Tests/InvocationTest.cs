using NUnit.Framework;
using System;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class InvocationTest
    {
        private Invocation invocation;

        [SetUp]
        public void SetUp()
        {
            invocation = new Invocation("some.js");
        }

        [Test]
        public void Invocation_Constructor_SetsJavaScriptFile()
        {
            // Assert
            Assert.AreEqual("some.js", invocation.JavaScriptFile);
        }

        [Test]
        public void Invocation_Constructor_InitializesFunctionCalls()
        {
            // Assert
            Assert.IsNotNull(invocation.FunctionCalls);
            Assert.AreEqual(0, invocation.FunctionCalls.Count);
        }
    }
}
