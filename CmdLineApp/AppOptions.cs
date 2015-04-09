using System;
using CommandLine;
using CommandLine.Text;

namespace CmdLineApp
{
    public class AppOptions
    {
        [Option('v', "verbose", DefaultValue = false, HelpText = "generate more output")]
        public bool Verbose { get; set; }

        [Option('d', "directory", Required = true, HelpText = "directory to list")]
        public string Directory { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                helptext => HelpText.DefaultParsingErrorsHandler(this, helptext)
            );
        }
    }
}
