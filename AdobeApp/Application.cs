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
        public Options Options { get; set; }

        public AppleScriptBuilder AppleScriptBuilder { get; set; }

        /// <summary>
        /// Constructs an application with a name
        /// </summary>
        /// <param name="name">Name.</param>
        public Application(string applicationName)
        {
            Options = new Options
            {
                ApplicationName = applicationName,
                Timeout = 1800
            };

            AppleScriptBuilder = new AppleScriptBuilder(Options);
        }

        /// <summary>
        /// Returns an invocation to get filled with function calls.
        /// </summary>
        /// <returns>a dynamic object usable for chained JavaScript calls</returns>
        /// <param name="javaScriptFile">Java script file which contains all functions called</param>
        public dynamic Invocation(string javaScriptFile)
        {
            return new Invocation(javaScriptFile);
        }

        /// <summary>
        /// Invokes a previously created and filled invocation
        /// </summary>
        /// <returns>data structure returned by function call</returns>
        /// <param name="invocation">Invocation.</param>
        public object Invoke(Invocation invocation)
        {
            using (var js = new JavaScriptDir())
            {
                var appleScript = AppleScriptBuilder.GenerateAppleScript(js.JavaScript(invocation.JavaScriptFile), invocation);
                var serializedResult = RunAppleScript(appleScript);
                return DeserializeResult(serializedResult);
            }
        }

        /// <summary>
        /// Runs the AppleScript
        /// </summary>
        /// <returns>Result of the AppleScript as Text</returns>
        /// <param name="appleScript">AppleScript source code</param>
        public string RunAppleScript(string appleScript)
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
