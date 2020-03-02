using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.SolutionDiscovery;
using Paczker.Infrastructure;

namespace Paczker.Core.Commands
{
    public static class ListDependentProjectsCommand
    {
        public static Result<IEnumerable<string>> ListDependentProjects(string path, string projectName)
        {
            LoggerFactory.LogInfo($"Dependant projects to {projectName}");
            var result = ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => DependencyTree.FindReferences(x, projectName))
                .Map(x => x.Select(y => y.Name));

            result.Map(x => x.Iter(y => LoggerFactory.LogInfo(y)));

            return result;
        }
    }
}