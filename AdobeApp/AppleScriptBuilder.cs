using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AdobeApp
{
    /// <summary>
    /// Takes care for building the AppleScript code needed to run JavaScript
    /// </summary>
    public class AppleScriptBuilder
    {
        private readonly string SCRIPT_ARGUMENTS = "script_arguments";

        /// <summary>
        /// Options.
        /// </summary>
        /// <value>Options for controlling the AppleScript generation</value>
        public Options Options { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdobeApp.AppleScriptBuilder"/> class.
        /// </summary>
        public AppleScriptBuilder(Options options)
        {
            Options = options;
        }

        /// <summary>
        /// Emit a "do javascript" statement (typically overloaded)
        /// </summary>
        /// <returns>the AppleScript statement for running a javaScript</returns>
        /// <param name="javaScriptFile">Java script file.</param>
        /// <param name="arguments">Name of the variable containing JSON serialized arguments</param>
        public virtual string DoJavaScript(string javaScriptFile, string arguments)
        {
            // FIXME: create overloaded version of this class for InDesign, PhotoShop, etc.

            if (Options.ApplicationName.Contains("InDesign"))
            {
                return String.Format(
                    "do script (POSIX file \"{0}\") language javascript with arguments {1} undo mode fast entire script", 
                    javaScriptFile, SCRIPT_ARGUMENTS
                );
            }
            else
            {
                return String.Format(
                    "do javascript \"$.evalFile('{0}')\" with arguments {1}",
                    javaScriptFile, SCRIPT_ARGUMENTS
                );
            }    
        }

        private string GenerateFunctionCalls(object functionCalls)
        {
            return JsonConvert.SerializeObject(functionCalls);
        }

        /// <summary>
        /// Generates the complete AppleScript.
        /// </summary>
        /// <returns>The generated AppleScript.</returns>
        /// <param name="javaScriptFile">Java script file.</param>
        /// <param name="invocation">The functions and parameters to call</param>
        public string GenerateAppleScript(string javaScriptFile, Invocation invocation)
        {
            var scriptArguments = GenerateFunctionCalls(invocation.FunctionCalls);
            var appleScript = new StringBuilder();

            appleScript.AppendLine(String.Format("tell application \"{0}\"", Options.ApplicationName));
            appleScript.AppendLine(String.Format("with timeout of {0} seconds", Options.Timeout));

            appleScript.AppendLine(ArgumentsAsAssignment(SCRIPT_ARGUMENTS, scriptArguments));

            appleScript.AppendLine(DoJavaScript(javaScriptFile, SCRIPT_ARGUMENTS));
                
            appleScript.AppendLine("end timeout");
            appleScript.AppendLine("end tell");

            return appleScript.ToString();
        }

        // encode Json String into a unicode string variable assignment that can be put into our AppleScript
        private string ArgumentsAsAssignment(string variable, string arguments)
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
        private string ToUtxt(string content)
        {
            string encodedContent =
                String.Join(
                    "", 
                    content.ToCharArray().Select(c => ((int)c).ToString("X4"))
                );

            // FIXME: is unicode encoding here correct?
            return String.Format("\u00c7data utxt{0}\u00c8 as Unicode text", encodedContent);
        }

        private IEnumerable<string> SplitIntoChunks(string content, int chunkSize = 40)
        {
            for (int i = 0; i < content.Length; i += chunkSize)
            {
                var length = i + chunkSize > content.Length
                    ? content.Length - i
                    : chunkSize;

                yield return content.Substring(i, length);
            }
        }

    }
}
