using System.Collections.Generic;
using CommandLine;
using Paczker.Core.VersionOperators;

namespace Paczker.Verbs
{
    [Verb("dec", HelpText = "Decrements packages versions of specified package and dependants package projects.")]
    public class DecOptions
    {
        [Option('s', "solution", Required = true, HelpText = "Path to a solution file.")]
        public string SolutionPath { get; set; }

        [Option('p', "projects", Required = true, HelpText = "Base projects to decrement version.")]
        public IEnumerable<string> ProjectNames { get; set; }
        
        [Option('n', "version", Required = true, HelpText = "Version part to change.")]
        public VersionPart VersionPart { get; set; }
    }
}