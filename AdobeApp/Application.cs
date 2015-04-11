using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace AdobeApp
{
    /// <summary>
    /// Application represents an Adobe Application getting controlled by JavaScript
    /// </summary>
    /// <example>
    /// var indesign = new Application("Adobe InDesign CC");
    /// 
    /// var render = indesign.Invocation("render.js")
    ///     .Open("/path/to/x.indd")
    ///     .EnsureFontsAreLoaded()
    ///     .EnsureImagesAreLinked()
    ///     .CreateProof()
    ///     .Close();
    /// 
    /// var result = indesign.Invoke(render);
    /// </example>
    public class Application
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The full name of the application</value>
        public string Name { get; set; }

        public Application(string name)
        {
            Name = name;
        }

        public dynamic Invocation(string javaScriptFile)
        {
            return new Invocation(javaScriptFile);
        }

        public object Invoke(Invocation invocation)
        {
            throw new NotImplementedException();

            // - JavaScript Dateien sammeln bottom-up bis zur obersten Assembly
            //   in umgekehrter Stack-Aufrufreihenfolge. Kann man die ermitteln?
            // - Datenstruktur von Invocation als JSON erzeugen
            // - AppleScript erzeugen
            // - AppleScript starten
            // - Rückgabewerte deserialisieren
        }

        // actually: list assemblies in call stack
        public IEnumerable<Assembly> ListAssemblies()
        {
            var stack = new StackTrace();

            return stack.GetFrames()
                .Select(f => f.GetMethod().DeclaringType.Assembly)
                .Where(a => !a.GetName().Name.StartsWith("mscorlib"))
                .Distinct();
        }

        public IEnumerable<string> ListJavaScriptResources(Assembly assembly)
        {
            return assembly.GetManifestResourceNames()
                .Where(r => r.IndexOf(".javascript.", StringComparison.OrdinalIgnoreCase) > 0);
        }

        public string LoadJavaScriptResource(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
