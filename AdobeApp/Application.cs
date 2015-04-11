using Newtonsoft.Json;
using TwoPS.Processes;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        // Hint: Resource names are dot-separated.
        private readonly string JAVASCRIPT_FOLDER_NAME = ".javascript.";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The full name of the application</value>
        public string Name { get; set; }

        public int Timeout { get; set; }

        public Application(string name)
        {
            Name = name;
            Timeout = 1800;
        }

        public dynamic Invocation(string javaScriptFile)
        {
            return new Invocation(javaScriptFile);
        }

        public object Invoke(Invocation invocation)
        {
            using (var dir = new JavaScriptDir())
            {
                CollectJavaScriptFiles(dir);
                var functionCalls = GenerateFunctionCalls(invocation);
                var appleScript = GenerateAppleScript(dir.JavaScript(invocation.JavaScriptFile), functionCalls);
                var serializedResult = RunAppleScript(appleScript);
                return DeserializeResult(serializedResult);
            }
        }

        // public for easier testability
        public void CollectJavaScriptFiles(JavaScriptDir dir)
        {
            foreach (var assembly in ListAssemblies())
            {
                foreach (var resource in ListJavaScriptResources(assembly))
                {
                    int folderPosition = resource.LastIndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase);
                    var fileName = resource.Substring(folderPosition + JAVASCRIPT_FOLDER_NAME.Length);

                    dir.SaveJavaScript(fileName, LoadJavaScriptResource(assembly, resource));
                }
            }
        }

        public string GenerateFunctionCalls(Invocation invocation)
        {
            return JsonConvert.SerializeObject(invocation.FunctionCalls);
        }

        public string GenerateAppleScript(string javaScriptFile, string scriptArguments)
        {
            var appleScript = new StringBuilder();

            appleScript.AppendLine(String.Format("tell application \"{0}\"", Name));
            appleScript.AppendLine(String.Format("with timeout of {0} seconds", Timeout));

            appleScript.AppendLine(ArgumentsAsAssignment("script_arguments", scriptArguments));

            if (Name.Contains("InDesign"))
            {
                appleScript.AppendLine(
                    String.Format(
                        "do script (POSIX file \"{0}\") language javascript with arguments script_arguments undo mode fast entire script", 
                        javaScriptFile
                    )
                );
            }
            else
            {
                appleScript.AppendLine(
                    String.Format(
                        "do javascript \"$.evalFile('{0}')\" with arguments script_arguments",
                        javaScriptFile
                    )
                );
            }    

            appleScript.AppendLine("end timeout");
            appleScript.AppendLine("end tell");

            return appleScript.ToString();
        }

        // encode Json String into a unicode string variable assignment that can be put into our AppleScript
        public string ArgumentsAsAssignment(string variable, string arguments)
        {
            var uTxt = new StringBuilder();
            uTxt.AppendLine(
                String.Format("set {0} to \"\"", variable)
            );

            foreach (var chunk in SplitIntoChunks(arguments))
            {
                uTxt.AppendLine(
                    String.Format("set {0} to {0} & {1}", variable, ToUtxt(chunk))
                );
            }

            return uTxt.ToString();
        }

        // encode a single string into a data utxt applescript string
        public string ToUtxt(string content)
        {
            string encodedContent =
                String.Join(
                    "", 
                    content.ToCharArray().Select(c => ((int)c).ToString("X4"))
                );

            // FIXME: is unicode encoding here correct?
            return String.Format("\u00c7data utxt{0}\u00c8 as Unicode text", encodedContent);
        }

        public  IEnumerable<string> SplitIntoChunks(string content, int chunkSize = 40)
        {
            for (int i = 0; i < content.Length; i += chunkSize)
            {
                var length = i + chunkSize > content.Length
                    ? content.Length - i
                    : chunkSize;
                
                yield return content.Substring(i, length);
            }
        }

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

        public object DeserializeResult(string serializedResult)
        {
            return JsonConvert.DeserializeObject(serializedResult);
        }

        // actually: list assemblies in call stack but this is exactly
        // the order in which we want to load JavaScript files
        public IEnumerable<Assembly> ListAssemblies()
        {
            var stack = new System.Diagnostics.StackTrace();

            return stack.GetFrames()
                .Select(f => f.GetMethod().DeclaringType.Assembly)
                .Where(a => !a.GetName().Name.StartsWith("mscorlib"))
                .Distinct();
        }

        public IEnumerable<string> ListJavaScriptResources(Assembly assembly)
        {
            return assembly.GetManifestResourceNames()
                .Where(r => r.IndexOf(JAVASCRIPT_FOLDER_NAME, StringComparison.OrdinalIgnoreCase) > 0);
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
