using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.SetPreReleaseVersion
{
    public class SetPreReleaseVersionCommandHandler : ICommandHandler<SetPreReleaseVersionCommand>
    {
        public Result<IEnumerable<string>> Handle(SetPreReleaseVersionCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x).ToList();

            return Prelude.Optional(DependencyTree.FindReferences(projects, message.ProjectNames)
                    .Map(VersionModifier.SetPreReleaseVersion)
                    .Map(ProjectSaver.Save)
                    .Map(ProjectConverter.ToViewString)
                    .DefaultIfEmpty())
                .Match(x => new Result<IEnumerable<string>>(x),
                    new Result<IEnumerable<string>>(new NothingFoundException("no projects were found")));
        }
    }
}