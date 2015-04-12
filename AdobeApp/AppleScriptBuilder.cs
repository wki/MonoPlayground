using System;
using System.Text;
using Newtonsoft.Json;

namespace AdobeApp
{
    public static class AppleScriptBuilder
    {
        public static string DoJavaScript(string javaScriptFile, object functionCalls)
        {
            throw new NotImplementedException();
        }

        private string GenerateFunctionCalls(object functionCalls)
        {
            return JsonConvert.SerializeObject(functionCalls);
        }

        private static string GenerateAppleScript(string javaScriptFile, string scriptArguments)
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
        private static string ArgumentsAsAssignment(string variable, string arguments)
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
        private static string ToUtxt(string content)
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
