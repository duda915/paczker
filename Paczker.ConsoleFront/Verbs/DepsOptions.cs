using System.Collections.Generic;
using CommandLine;

namespace Paczker.Verbs
{
    [Verb("deps", HelpText = "List dependants packages of package project.")]
    public class DepsOptions
    {
        [Option('s', "solution", Required = true, HelpText = "Path to a solution file.")]
        public string SolutionPath { get; set; }
        
        [Option('p', "projects", Required = true, HelpText = "Project names to list dependants.")]
        public IEnumerable<string> ProjectNames { get; set; }
    }
}