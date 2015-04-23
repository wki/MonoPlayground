using System;
using TwoPS.Processes;
using System.Text;
using System.Threading.Tasks;

namespace AdobeApp
{
    /// <summary>
    /// Execute an applescript from various sources:
    ///  * given as a string
    ///  * given from an AppleScriptBuilder
    ///  * from a file saved inside a script dir
    /// </summary>
    public class AppleScriptRunner
    {
        private const string OSASCRIPT = "/usr/bin/osascript";
        private readonly ScriptDir ScriptDir;

        public AppleScriptRunner()
        {
        }

        public AppleScriptRunner(ScriptDir scriptDir)
        {
            ScriptDir = scriptDir;
        }

        public AppleScriptRunner(string dir)
        {
            ScriptDir = new ScriptDir(dir);
        }

        /// <summary>
        /// Runs the script.
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="script">Script.</param>
        public string RunScript(string script)
        {
            var options = new ProcessOptions(OSASCRIPT, "-");
            options.StandardInputEncoding = Encoding.GetEncoding("macintosh");
            // stdout is UTF8, because we pass-thru JavaScript's output

            var process = new Process(options);
            var task = Task.Run(() => process.Run());
            process.StandardInput.Write(script);
            process.StandardInput.Close();
            var result = task.Result;

            if (result.ExitCode != 0)
            {
                throw new AppleScriptException(result.StandardError);
            }

            return result.StandardOutput;
        }

        /// <summary>
        /// runs the script represented by the builder
        /// </summary>
        /// <returns>The script.</returns>
        /// <param name="builder">Builder.</param>
        public string RunScript(AppleScriptBuilder builder)
        {
            return RunScript(builder.Build());
        }
    }
}

