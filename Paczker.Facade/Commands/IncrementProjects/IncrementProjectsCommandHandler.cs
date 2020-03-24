using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using Paczker.Core;
using Paczker.Core.Nuget;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.IncrementProjects
{
    public class IncrementProjectsCommandHandler : ICommandHandler<IncrementProjectsCommand>
    {
        public Result<IEnumerable<string>> Handle(IncrementProjectsCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x).ToList();

            return Optional(DependencyTree.FindReferences(projects, message.ProjectNames)
                    .Select(x => VersionModifier.IncrementVersion(x, message.Version))
                    .Select(ProjectSaver.Save)
                    .Select(ProjectConverter.ToViewString)
                    .DefaultIfEmpty())
                .Match(x => new Result<IEnumerable<string>>(x),
                    new Result<IEnumerable<string>>(new NothingFoundException("no projects were found")));
        }
    }
}