using System.Collections.Generic;
using LanguageExt;

namespace Paczker.Core
{
    public class Project
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public Option<string> AssemblyVersion { get; set; }
        public string Path { get; set; }
        public IEnumerable<Project> Dependencies { get; set; }
    }
}