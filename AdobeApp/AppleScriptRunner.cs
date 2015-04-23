using System;

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
            // TODO: implement me
            //
            // encode the Script into macroman,
            // transport to osascript
            // collect and return result
            throw new NotImplementedException();
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

