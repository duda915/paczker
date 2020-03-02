using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.ProjectManipulator;
using static LanguageExt.Prelude;

namespace Paczker.Core.SolutionDiscovery
{
    public static class ProjectsMapper
    {
        public static Option<Project> MapCsProj(string path)
        {
            var doc = ProjectLoader.Load(path);
            var version = NodeFinder.GetVersionNodeValue(doc, VersionNode.Version);
            var assemblyVersion = NodeFinder.GetVersionNodeValue(doc, VersionNode.AssemblyVersion);
            var dependencies = NodeFinder.GetProjectReferenceNodeValues(doc);

            if (version.IsNone)
            {
                return None;
            }

            var dependenciesPaths = GetDependenciesPaths(path, dependencies);
            
            var dependenciesProjects = dependenciesPaths.Select(MapCsProj)
                .Where(x => x.IsSome).Select(x => x.ValueUnsafe());

            return Some(
                new Project
                {
                    Name = path.Split("/").Last(),
                    Version = version.ValueUnsafe(),
                    AssemblyVersion = assemblyVersion,
                    Path = path,
                    Dependencies = dependenciesProjects
                });
        }

        private static IEnumerable<string> GetDependenciesPaths(string projectPath, IEnumerable<string> relativePaths)
        {
            if (!relativePaths.Any())
            {
                return new string[0];
            }

            var projectDirectoryPath = $"{Path.GetDirectoryName(projectPath)}";

            return relativePaths.Select(x => $"{projectDirectoryPath}/{x}");
        }
    }
}