using System;
using System.Dynamic;
using System.Collections.Generic;

namespace AdobeApp
{
    /// <summary>
    /// Invocation.
    /// </summary>
    public class Invocation : DynamicObject
    {
        /// <summary>
        /// Gets or sets the java script file.
        /// </summary>
        /// <value>The JavaScript file to run</value>
        public string JavaScriptFile { get; set; }

        public List<FunctionCall> FunctionCalls { get; set; }

        public Invocation(string javaScriptFile)
        {
            JavaScriptFile = javaScriptFile;
            FunctionCalls = new List<FunctionCall>();
        }

        public Invocation Try(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

        public Invocation Catch(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

        public Invocation Finally(Func<Invocation, Invocation> invocation)
        {
            throw new NotImplementedException();
        }

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
