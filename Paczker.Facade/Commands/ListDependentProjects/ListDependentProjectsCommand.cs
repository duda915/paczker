
using System;
using System.Collections.Generic;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.ListDependentProjects
{
    public class ListDependentProjectsCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyCollection<string> ProjectNames;

        public ListDependentProjectsCommand(string slnPath, IReadOnlyCollection<string> projectNames)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
        }
    }
}