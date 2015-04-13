using System;

namespace AdobeApp
{
    /// <summary>
    /// Occurs when the execution of an AppleScript dies
    /// </summary>
	public class AppleScriptException : Exception
	{
        public AppleScriptException(String message) : base(message)
        {
        }
	}

}
