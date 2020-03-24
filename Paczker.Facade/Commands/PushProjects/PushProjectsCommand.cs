using System.Collections.Generic;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.PushProjects
{
    public class PushProjectsCommand : ICommand
    {
        public readonly string SlnPath;
        public readonly IReadOnlyCollection<string> ProjectNames;
        public readonly string Source;
        public readonly string BuildProfile;

        public PushProjectsCommand(string slnPath, IReadOnlyCollection<string> projectNames, string source, string buildProfile)
        {
            SlnPath = slnPath;
            ProjectNames = projectNames;
            Source = source;
            BuildProfile = buildProfile;
        }
    }
}