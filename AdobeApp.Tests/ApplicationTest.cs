using NUnit.Framework;
using AdobeApp;
using System;
using System.Linq;
using System.Reflection;
using System.IO;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class ApplicationTest
    {
        private Application app;

        [SetUp]
        public void SetUp()
        {
            app = new Application("Foo");
        }

        [Test]
        public void AdobeApp_Constructor_SetsName()
        {
            // Assert
            Assert.AreEqual("Foo", app.Options.ApplicationName);
        }

        [Test]
        public void AdobeApp_Invocation_CreatesInvocationObject()
        {
            // Act
            var invocation = app.Invocation("some.js");

            // Assert
            Assert.IsNotNull(invocation);
        }

        [Test]
        public void AdobeApp_InvocationChaining_CreatesFunctionEntries()
        {
            // Act
            var invocation =
                app.Invocation("some.js")
                    .Open("foo.indd")
                    .DoSomething("foo", "bar");

            // Assert
            Assert.AreEqual(2, ((Invocation)invocation).FunctionCalls.Count);
        }
    }
}
