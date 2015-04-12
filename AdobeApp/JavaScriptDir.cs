using System;using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdobeApp
{
    public class JavaScriptDir: IDisposable
    {
        // Hint: Resource names are dot-separated.
        private readonly string JAVASCRIPT_FOLDER_NAME = ".javascript.";

        public string Dir { get; set; }

        public JavaScriptDir()
        {
            Dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(Dir);

            CollectJavaScriptFiles();

        }

        public void SaveJavaScript(string fileName, string content)
        {
            File.WriteAllText(JavaScript(fileName), content);
        }

        public string JavaScript(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }


        internal void CollectJavaScriptFiles()
        {
            foreach (var assembly in ListAssemblies())
            {
                foreach (var resource in ListJavaScriptResources(assembly))
                {
                    int folderPosition = resource.LastIndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase);
                    var fileName = resource.Substring(folderPosition + JAVASCRIPT_FOLDER_NAME.Length);

                    SaveJavaScript(fileName, LoadJavaScriptResource(assembly, resource));
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

        private string LoadJavaScriptResource(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public void Dispose()
        {
            Directory.Delete(Dir, true);
        }
    }
}
