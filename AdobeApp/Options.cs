using System;

namespace AdobeApp
{
    /// <summary>
    /// Holds various option values used at different places.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Name of the application
        /// </summary>
        /// <value>The full name of the application eg "Adobe InDesign CC"</value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Applescript Timeout
        /// </summary>
        /// <value>Timeout for the execution of the AppleScript</value>
        public int Timeout { get; set; }
    }
}

