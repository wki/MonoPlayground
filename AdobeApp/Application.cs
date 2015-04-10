using System;

namespace AdobeApp
{
    /// <summary>
    /// Application represents an Adobe Application
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
            //   in Stack-Aufrufreihenfolge. Kann man die ermitteln?
            // - Datenstruktur von Invocation als JSON erzeugen
            // - AppleScript erzeugen
            // - AppleScript starten
            // - Rückgabewerte deserialisieren
        }
    }
}
