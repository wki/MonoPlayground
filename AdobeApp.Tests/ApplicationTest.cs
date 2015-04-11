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

        [Test]
        public void AdobeApp_CopyJavascriptFilesTo_FillsDir()
        {
            // Arrange
            using (var js = new JavaScriptDir())
            {
                // Act
                app.CollectJavaScriptFiles(js);

                // Assert
                Assert.IsTrue(File.Exists(Path.Combine(js.Dir, "dummy.js")));
                Assert.IsTrue(File.Exists(Path.Combine(js.Dir, "adobe.js")));
            };
        }

        [Test]
        public void AdobeApp_GenerateFunctionCalls_LooksGood()
        {
            // Arrange
            var invocation =
                app.Invocation("some.js")
                    .Open("foo.indd")
                    .DoSomething("foo", "bar");

            // Act
            var functionCalls = app.GenerateFunctionCalls(invocation);
            Console.WriteLine(functionCalls);

            // Assert
            Assert.AreEqual(
                "[{\"Name\":\"Open\",\"Arguments\":[{\"Path\":\"foo.indd\"}]},{\"Name\":\"DoSomething\",\"Arguments\":[\"foo\",\"bar\"]}]",
                functionCalls
            );
        }

        [Test]
        public void AdobeApp_ToUtxtFromStringABC_CreatesDataLine()
        {
            // Act
            var line = app.ToUtxt("ABC");

            // Assert
            Assert.AreEqual(
                "\u00c7data utxt004100420043\u00c8 as Unicode text",
                line
            );
        }

        [Test]
        public void AdobeApp_ToUtxtFromStringUmlaut_CreatesDataLine()
        {
            // Act
            var line = app.ToUtxt("ÖÄU");

            // Assert
            Assert.AreEqual(
                "\u00c7data utxt00D600C40055\u00c8 as Unicode text",
                line
            );
        }

        [Test]
        public void AdobeApp_ArgumentsAsAssignment_CreatesAssignments()
        {
            // Act
            var assignment = app.ArgumentsAsAssignment("script_args", "012345689.12345689.12345689.12345689.12345689");
            Console.WriteLine(assignment);

            // Assert
            Assert.IsTrue(assignment.StartsWith("set script_args to"));

            // TODO: must have 3 lines
        }

        [Test]
        public void AdobeApp_GenerateAppleScript_CreatesScript()
        {
            // Arrange
            app.Name = "Adobe InDesign CC";

            // Act
            var appleScript = app.GenerateAppleScript("foo.js", "\"asdf\"");
            Console.WriteLine(appleScript);

            // Assert
            Assert.IsTrue(true);

            // TODO: add some more tests
        }
    }
}
