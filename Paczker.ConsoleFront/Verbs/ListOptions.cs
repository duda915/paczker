using CommandLine;

namespace Paczker.Verbs
{
    [Verb("list", HelpText = "List all packages projects in solution.")]
    public class ListOptions
    {
        [Option('s', "solution", Required = true, HelpText = "Path to a solution file.")]
        public string SolutionPath { get; set; }
    }
}