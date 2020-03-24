using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;
using static LanguageExt.Prelude;

namespace Paczker.Core.SolutionDiscovery
{
    public static class ProjectsFactory
    {
        public static Option<Project> MapCsProj(string name, string path)
        {
            var version = NodeFinder.GetVersionNodeValue(path, VersionNode.Version)
                .Bind(VersionFactory.CreateVersion);
            var assemblyVersion = NodeFinder.GetVersionNodeValue(path, VersionNode.AssemblyVersion)
                    .Bind(VersionFactory.CreateAssemblyVersion);

            return version.Bind(x => Some(new Project(name, x, 
                assemblyVersion, path)));
        }
    }
}