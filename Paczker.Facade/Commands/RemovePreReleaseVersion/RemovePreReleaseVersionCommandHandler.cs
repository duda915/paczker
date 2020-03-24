using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.RemovePreReleaseVersion
{
    public class RemovePreReleaseVersionCommandHandler : ICommandHandler<RemovePreReleaseVersionCommand>
    {
        public Result<IEnumerable<string>> Handle(RemovePreReleaseVersionCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x).ToList();

            return Prelude.Optional(DependencyTree.FindReferences(projects, message.ProjectNames)
                    .Select(VersionModifier.RemovePreReleaseVersion)
                    .Select(ProjectSaver.Save)
                    .Select(ProjectConverter.ToViewString)
                    .DefaultIfEmpty())
                .Match(x => new Result<IEnumerable<string>>(x),
                    new Result<IEnumerable<string>>(new NothingFoundException("no projects were found")));
        }
    }
}