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
            Assert.AreEqual("some.js", invocation.JavaScriptFileName);
        }

        [Test]
        public void Invocation_Constructor_InitializesFunctionCalls()
        {
            // Assert
            Assert.IsNotNull(invocation.FunctionCalls);
            Assert.AreEqual(0, invocation.FunctionCalls.Count);
        }

        [Test]
        public void Invocation_Open_AddsOpenFunctionCall()
        {
            // Act
            invocation.Open("mydocument.indd");

            // Assert
            Assert.AreEqual(1, invocation.FunctionCalls.Count);
            Assert.AreEqual("Open", invocation.FunctionCalls[0].Name);
            // Assert.AreEqual("mydocument.indd", invocation.FunctionCalls[0].Arguments[0].Path);
        }

        [Test]
        public void Invocation_SaveAs_AddsSaveFunctionCall()
        {
            // Act
            invocation.SaveAs("mydocument.indd");

            // Assert
            Assert.AreEqual(1, invocation.FunctionCalls.Count);
            Assert.AreEqual("Save", invocation.FunctionCalls[0].Name);
//            Assert.AreEqual(
//                new Object[]{ new { Path = "mydocument.indd"} },
//                invocation.FunctionCalls[0].Arguments
//            );
        }

        [Test]
        public void Invocation_Close_AddsCloseFunctionCall()
        {
            // Arrange
            invocation.FunctionCalls.Add(null);

            // Act
            invocation.Close();

            // Assert
            Assert.AreEqual(2, invocation.FunctionCalls.Count);
            Assert.AreEqual("Close", invocation.FunctionCalls[1].Name);
        }

        [Test]
        public void Invocation_CustomFunctionCall_AddsFunctionCall()
        {
            // Arrange
            // this ensures all other situations still offer Intellisense
            dynamic i = invocation;

            // Act
            i.DoSomething("foo", "bar");

            // Assert
            Assert.AreEqual(1, invocation.FunctionCalls.Count);
            Assert.AreEqual("DoSomething", invocation.FunctionCalls[0].Name);
            Assert.AreEqual(
                new object[] { "foo", "bar" },
                invocation.FunctionCalls[0].Arguments
            );
        }
    }
}
