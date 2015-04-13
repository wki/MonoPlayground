using System;

namespace AdobeApp
{
    /// <summary>
    /// Contains all neccesary options for a function call
    /// </summary>
	public class FunctionCall
	{
        /// <summary>
        /// Name of the function to call
        /// </summary>
        /// <value>The name of the function inside the global scope of JavaScript</value>
        public string Name { get; set; }

        /// <summary>
        /// Arguments for the function call
        /// </summary>
        /// <value>List of arguments for the function call.</value>
        public object[] Arguments { get; set; }
	}
}
