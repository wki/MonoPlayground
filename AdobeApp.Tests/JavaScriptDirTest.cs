using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace AdobeApp.Tests
{
    [TestFixture]
    public class JavaScriptDirTest
    {
        public JavaScriptDir js { get; set; }

        [SetUp]
        public void SetUp()
        {
            js = new JavaScriptDir();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                js.Dispose();
            }
            catch (DirectoryNotFoundException)
            {
                // ignored in the case we call dispose ourselves
            }
        }

        [Test]
        public void JavaScriptDir_Constructor_CreatesTempDir()
        {
            // Assert
            Assert.IsTrue(Directory.Exists(js.Dir));
        }

        [Test]
        public void JavaScriptDir_Constructor_CreateCollectsFiles()
        {
            // Arrange
            var files =
                Directory.EnumerateFiles(js.Dir)
                    .Select(p => Path.GetFileName(p));
            
            Console.WriteLine(String.Join(", ", files));

            // Assert
            Assert.IsTrue(files.Contains("adobe.js"), "adobe.js");
            Assert.IsTrue(files.Contains("dummy.js"), "dummy.js");
        }

        [Test]
        public void JavaScriptDir_Dispose_RemovesTempDir()
        {
            // Arrange
            File.WriteAllText(Path.Combine(js.Dir, "foo.js"), "hi");

            // Act
            js.Dispose();

            // Assert
            Assert.IsFalse(Directory.Exists(js.Dir));
        }

        [Test]
        public void JavaScriptDir_SaveJavaScript_CreatesFile()
        {
            // Act
            js.SaveJavaScript("foo.js", "/* foo */");

            // Assert
            var path = Path.Combine(js.Dir, "foo.js");
            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual("/* foo */", File.ReadAllText(path));
        }

        [Test]
        public void JavaScriptDir_JavaScript_ReturnsFullPathToFile()
        {
            // Act
            var path = js.JavaScript("bar.js");

            // Assert
            var expectedPath = Path.Combine(js.Dir, "bar.js");
            Assert.AreEqual(path, expectedPath);
        }
    }
}
