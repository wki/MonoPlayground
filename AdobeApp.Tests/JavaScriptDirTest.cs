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
            Assert.AreEqual(0, Directory.EnumerateFiles(js.Dir).Count());
            Assert.AreEqual(0, Directory.EnumerateDirectories(js.Dir).Count());
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
            var fileName = Path.Combine(js.Dir, "foo.js");
            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("/* foo */", File.ReadAllText(fileName));
        }
    }
}
