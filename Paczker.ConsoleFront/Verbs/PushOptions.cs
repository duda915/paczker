using System.Collections.Generic;
using CommandLine;

namespace Paczker.Verbs
{
    [Verb("push", HelpText = "Pushes package and dependants to specified source using dotnet nuget.")]
    public class PushOptions
    {
        [Option('s', "solution", Required = true, HelpText = "Path to a solution file.")]
        public string SolutionPath { get; set; }
        
        [Option('p', "projects", Required = true, HelpText = "Base project names to push.")]
        public IEnumerable<string> ProjectNames { get; set; }
        
        [Option('r', "source", Required = true, HelpText = "Nuget feed source.")]
        public string Source { get; set; }

        [Option('c', "bconfiguration", Required = true, HelpText = "Build configuration of projects.")]
        public string BuildConfiguration { get; set; }
    }
}