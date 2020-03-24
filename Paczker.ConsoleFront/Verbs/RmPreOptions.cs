using System.Collections.Generic;
using CommandLine;

namespace Paczker.Verbs
{
    [Verb("rmpre", HelpText = "Removes pre-release version of base package and dependants")]
    public class RmPreOptions
    {
        [Option('s', "solution", Required = true, HelpText = "Path to a solution file.")]
        public string SolutionPath { get; set; }
        
        [Option('p', "projects", Required = true, HelpText = "Project names to remove pre-release version.")]
        public IEnumerable<string> ProjectNames { get; set; }
    }
}