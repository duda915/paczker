using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.SolutionDiscovery;
using Paczker.Infrastructure;

namespace Paczker.Core.Commands
{
    public class ListAllProjectsCommand
    {
        public static Unit ListAllProjects(string path)
        {
            LoggerFactory.LogInfo($"Listing all projects");
            ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => x.Select(y => y.Name))
                .Map(x => x.Iter(y => LoggerFactory.LogInfo(y)));
            
            return Unit.Default;
        }
    }
}