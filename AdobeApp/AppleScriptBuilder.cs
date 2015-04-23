using System;
using System.Collections.Generic;

namespace AdobeApp
{
    /// <summary>
    /// Apple script builder.
    /// </summary>
    public class AppleScriptBuilder
    {
        private List<string> headCalls;
        private List<string> tailCalls;

        private Boolean isInDesign = false;

        public AppleScriptBuilder()
        {
            headCalls = new List<string>();
            tailCalls = new List<string>();
        }

        /// <summary>
        /// build the applescript from the previously collected calls
        /// </summary>
        public string Build()
        {
            var lines = new List<string>(headCalls);
            lines.AddRange(tailCalls);

            return String.Join("\n", lines)
        }

        #region builders
        public AppleScriptBuilder Tell(string application)
        {
            isInDesign = application.Contains("InDesign");

            headCalls.Add(String.Format("tell application \"{0}\"", application));
            tailCalls.Insert(0, "end tell");

            return this;
        }

        public AppleScriptBuilder Timeout(int timeout)
        {
            headCalls.Add(String.Format("with timeout of {0} seconds", timeout));
            tailCalls.Insert(0, "end timeout");

            return this;
        }

        public AppleScriptBuilder Assign(string variableName, string content)
        {
            headCalls.Add(String.Format("set {0} to {1}", variableName, content));

            return this;
        }

        public AppleScriptBuilder RunJavaScriptFile(string file, string args) 
        {
            if (isInDesign)
            {
                headCalls.Add(
                    String.Format(
                        "do script (POSIX file \"{0}\") language javascript with arguments {1} undo mode fast entire script", 
                        file, args
                    )
                );
            }
            else
            {
                headCalls.Add(
                    String.Format(
                        "do javascript \"$.evalFile('{0}')\" with arguments {1}",
                        file, args
                    )
                );
            } 
            return this;
        }
        #endregion
    }
}

