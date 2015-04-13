using System;
using System.Dynamic;
using System.Collections.Generic;

namespace AdobeApp
{
    /// <summary>
    /// Encapsulates a complete JavaScript invocation chain
    /// </summary>
    public class Invocation : DynamicObject
    {
        /// <summary>
        /// JavaScript File containing functions
        /// </summary>
        /// <value>Name of the JavaScript file</value>
        public string JavaScriptFile { get; set; }

        /// <summary>
        /// All functions to call in sequence
        /// </summary>
        /// <value>A List of FunctionCall objects representing all functions to call</value>
        public List<FunctionCall> FunctionCalls { get; set; }

        /// <summary>
        /// Instantiates an invocation
        /// </summary>
        /// <param name="javaScriptFile">JavaScript file.</param>
        public Invocation(string javaScriptFile)
        {
            JavaScriptFile = javaScriptFile;
            FunctionCalls = new List<FunctionCall>();
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Try(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Catch(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="invocation">Invocation.</param>
        public Invocation Finally(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the file given
        /// </summary>
        /// <param name="path">Full path to file to open</param>
        /// <returns>Invocation to allow chaining</returns>
        public dynamic Open(string path)
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Name = "Open",
                    Arguments = new object[]
                    {
                        new { Path = path }
                    },
                }
            );

            return this;
        }

        /// <summary>
        /// Saves the open document
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        /// <param name="path">Path to save the file to</param>
        public dynamic SaveAs(string path)
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Name = "Save",
                    Arguments = new object[]
                    {
                        new { Path = path }
                    },
                }
            );

            return this;
        }

        /// <summary>
        /// Closes the currently open document without saving
        /// </summary>
        /// <returns>Invocation to allow chaining</returns>
        public dynamic Close()
        {
            FunctionCalls.Add(
                new FunctionCall
                {
                    Name = "Close",
                    Arguments = new object[] {},
                }
            );

            return this;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out dynamic result)
        {
            FunctionCalls.Add(
                new FunctionCall
                { 
                    Name = binder.Name,
                    Arguments = args
                }
            );

            result = this;
            return true;
        }
    }
}
