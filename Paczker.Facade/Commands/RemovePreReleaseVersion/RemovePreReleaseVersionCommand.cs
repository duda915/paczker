using System.Collections.Generic;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.RemovePreReleaseVersion
{
    public class RemovePreReleaseVersionCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyCollection<string> ProjectNames;

        public RemovePreReleaseVersionCommand(string slnPath, IReadOnlyCollection<string> projectNames)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
        }
    }
}