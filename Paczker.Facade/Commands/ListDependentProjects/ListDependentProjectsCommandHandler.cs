using System.Collections.Generic;
using System.Linq;
using LanguageExt.Common;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.ListDependentProjects
{
    public class ListDependentProjectsCommandHandler : ICommandHandler<ListDependentProjectsCommand>
    {
        public Result<IEnumerable<string>> Handle(ListDependentProjectsCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x)
                .ToList();

            var dependentProjects = DependencyTree.FindReferences(projects, message.ProjectNames)
                .Select(ProjectConverter.ToViewString).ToList();

            return dependentProjects.Any()
                ? new Result<IEnumerable<string>>(dependentProjects)
                : new Result<IEnumerable<string>>(new NothingFoundException("no projects were found"));
        }
    }
}