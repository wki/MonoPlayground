using System;
using System.Linq;
using TwoPS.Processes;
using System.Threading.Tasks;

namespace CmdLineApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var options = new AppOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Hello World!, dir={0}", options.Directory);

                ListDirectory2(options.Directory);
            }
            else
            {
                Console.WriteLine("Something went wrong: {0}",
                    String.Join(
                        "/",
                        options.LastParserState.Errors
                        .Select(e => e.BadOption.ShortName + ": " + e.ViolatesRequired)));
            }
        }

        public static void ListDirectory2(params string[] args)
        {
            var process = new Process("ls", args);
            var result = Task.Run(() => process.Run()).Result;

            if (result.ExitCode != 0)
            {
                Console.WriteLine("Received Error: ***{0}***, exit code: {1}",
                    result.StandardError, result.ExitCode
                );
            }
            else
            {
                Console.WriteLine("Received Output: ***{0}***",
                    result.StandardOutput
                );
            }
        }
    }
}
