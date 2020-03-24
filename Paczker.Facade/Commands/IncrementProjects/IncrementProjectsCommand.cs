using System.Collections.Generic;
using Paczker.Core.VersionOperators;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.IncrementProjects
{
    public class IncrementProjectsCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyList<string> ProjectNames;
        public readonly VersionPart Version;

        public IncrementProjectsCommand(string slnPath, IReadOnlyList<string> projectNames, VersionPart version)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
            Version = version;
        }
    }
}