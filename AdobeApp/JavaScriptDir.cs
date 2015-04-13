using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace AdobeApp
{
    /// <summary>
    /// A temporary directory holding all JavaScript files
    /// </summary>
    public class JavaScriptDir: IDisposable
    {
        // Hint: Resource names are dot-separated.
        private readonly string JAVASCRIPT_FOLDER_NAME = ".javascript.";

        /// <summary>
        /// Directory
        /// </summary>
        /// <value>Full path to the temporary directory</value>
        public string Dir { get; set; }

        /// <summary>
        /// Initializes a new JavaScriptDir instance, creates a temporary directory and collects JavaScript files
        /// </summary>
        public JavaScriptDir()
        {
            Dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(Dir);

            CollectJavaScriptFiles();

        }

        /// <summary>
        /// Initializes a new JavaScriptDir instance using an existing directory and collects JavaScript files
        /// </summary>
        /// <param name="dir">Full path to an existing directory</param>
        public JavaScriptDir(string dir)
        {
            Dir = dir;

            CollectJavaScriptFiles();
        }

        /// <summary>
        /// Writes the given content into a file inside the directory
        /// </summary>
        /// <param name="fileName">File name to save into</param>
        /// <param name="content">Content of the JavaScript file</param>
        public void SaveJavaScript(string fileName, string content)
        {
            File.WriteAllText(JavaScript(fileName), content);
        }

        /// <summary>
        /// Generates a path to a file inside dir.
        /// </summary>
        /// <returns>Full path to the file inside the directory</returns>
        /// <param name="fileName">File name.</param>
        public string JavaScript(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }

        private void CollectJavaScriptFiles()
        {
            foreach (var assembly in ListAssemblies())
            {
                foreach (var resource in ListJavaScriptResources(assembly))
                {
                    int folderPosition = resource.LastIndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase);
                    var fileName = resource.Substring(folderPosition + JAVASCRIPT_FOLDER_NAME.Length);

                    using (var resourceStream = assembly.GetManifestResourceStream(resource))
                    using (var javaScriptStream = new FileStream(JavaScript(fileName), FileMode.Create))
                    {
                        resourceStream.CopyTo(javaScriptStream);
                    }
                }
            }
        }

        // actually: list assemblies in call stack but this is exactly
        // the order in which we want to load JavaScript files
        private IEnumerable<Assembly> ListAssemblies()
        {
            var stack = new System.Diagnostics.StackTrace();

            return stack.GetFrames()
                .Select(f => f.GetMethod().DeclaringType.Assembly)
                .Where(a => !a.GetName().Name.StartsWith("mscorlib"))
                .Distinct();
        }

        private IEnumerable<string> ListJavaScriptResources(Assembly assembly)
        {
            return assembly.GetManifestResourceNames()
                .Where(r => r.IndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase) > 0);
        }

        public void Dispose()
        {
            Directory.Delete(Dir, true);
        }
    }
}
