using System.Collections.Generic;
using Paczker.Core.VersionOperators;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.DecrementProjects
{
    public class DecrementProjectsCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyCollection<string> ProjectNames;
        public readonly VersionPart Version;

        public DecrementProjectsCommand(string slnPath, IReadOnlyList<string> projectNames, VersionPart version)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
            Version = version;
        }
    }
}