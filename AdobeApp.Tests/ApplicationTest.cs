using NUnit.Framework;
using AdobeApp;
using System;
using System.Linq;
using System.Reflection;

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
            Assert.AreEqual("Foo", app.Name);
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

        [Test]
        public void AdobeApp_ListJavaScriptResources_GeneratesList()
        {
            // Arrange
            var assembly = typeof(Application).Assembly;

            // Act
            var scripts = app.ListJavaScriptResources(assembly);
                
            // Assert
            Assert.AreEqual(1, scripts.Count());
            Assert.AreEqual(1, scripts.Count(s => s.EndsWith(".adobe.js")));
        }

        [Test]
        public void AdobeApp_LoadJavaScriptResource_GivesResourceContent()
        {
            // Act
            var resource = 
                app.LoadJavaScriptResource(
                    Assembly.GetExecutingAssembly(),
                    "AdobeApp.Tests.JavaScript.dummy.js"
                );
            
            // Assert
            Assert.AreEqual("/* dummy.js */", resource);
        }

        [Test]
        public void AdobeApp_ListAssemblies_GivesAssemblyList()
        {
            // Act
            var assemblies = app.ListAssemblies();

            // Assert
            Assert.IsTrue(
                String.Join("/", 
                    assemblies.Select(a => a.GetName().Name)
                ).StartsWith("AdobeApp/AdobeApp.Tests")
            );
        }
    }
}
