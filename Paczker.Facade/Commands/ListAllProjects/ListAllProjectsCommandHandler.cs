using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.ListAllProjects
{
    public class ListAllProjectsCommandHandler : ICommandHandler<ListAllProjectsCommand>
    {
        public Result<IEnumerable<string>> Handle(ListAllProjectsCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x)
                .Map(ProjectConverter.ToViewString)
                .ToList();

            return projects.Any()
                ? new Result<IEnumerable<string>>(projects)
                : new Result<IEnumerable<string>>(new NothingFoundException("no projects were found"));
        }
    }
}