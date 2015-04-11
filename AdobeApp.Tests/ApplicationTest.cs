using NUnit.Framework;
using AdobeApp;
using System;

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
            // assert
            Assert.AreEqual("Foo", app.Name);
        }

        [Test]
        public void AdobeApp_Invocation_CreatesInvocationObject()
        {
            // Arrange
            var invocation = app.Invocation("some.js");

            // Assert
            Assert.IsNotNull(invocation);
        }
    }
}
