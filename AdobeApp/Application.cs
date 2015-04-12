using Newtonsoft.Json;
using TwoPS.Processes;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AdobeApp.Tests")]

namespace AdobeApp
{
    /// <summary>
    /// Represents an Adobe Program to be controlled by JavaScript
    /// </summary>
    /// <example>
    /// <code>
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
    /// </code>
    /// </example>
    public class Application
    {
        /// <summary>
        /// Contains the name of the application
        /// </summary>
        /// <value>The full name of the application like "Adobe InDesign CC"</value>
        public string Name { get; set; }

        /// <summary>
        /// The Timeout in seconds
        /// </summary>
        /// <value>The timeout for the applescript code.</value>
        public int Timeout { get; set; }

        /// <summary>
        /// Constructs an application with a name
        /// </summary>
        /// <param name="name">Name.</param>
        public Application(string name)
        {
            Name = name;
            Timeout = 1800;
        }

        /// <summary>
        /// Returns an invocation to get filled with function calls.
        /// </summary>
        /// <param name="javaScriptFile">Java script file which contains all functions called</param>
        public dynamic Invocation(string javaScriptFile)
        {
            return new Invocation(javaScriptFile);
        }

        /// <summary>
        /// Invokes a previously created and filled invocation
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        public object Invoke(Invocation invocation)
        {
            using (var js = new JavaScriptDir())
            {
                // FIXME: AppleScriptBuilder must be different for Adobe Versions
                var appleScript = AppleScriptBuilder.DoJavaScript(js.File(invocation.JavaScriptFile), invocation.FunctionCalls);
                var serializedResult = RunAppleScript(appleScript);
                return DeserializeResult(serializedResult);
            }
        }


        internal string RunAppleScript(string appleScript)
        {
            var process = new Process("/usr/bin/osascript", "-");
            var task = Task.Run(() => process.Run());
            process.StandardInput.Write(appleScript);
            process.StandardInput.Close();
            var result = task.Result;

            if (result.ExitCode != 0)
            {
                throw new AppleScriptException(result.StandardError);
            }

            return result.StandardOutput;
        }

        private object DeserializeResult(string serializedResult)
        {
            return JsonConvert.DeserializeObject(serializedResult);
        }
    }
}
