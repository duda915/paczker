using System.Collections.Generic;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.SetPreReleaseVersion
{
    public class SetPreReleaseVersionCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyCollection<string> ProjectNames;

        public SetPreReleaseVersionCommand(string slnPath, IReadOnlyCollection<string> projectNames)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
        }
    }
}