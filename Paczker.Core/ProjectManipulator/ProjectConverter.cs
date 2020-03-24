using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;

namespace Paczker.Core.ProjectManipulator
{
    public static class ProjectConverter
    {
        public static string ToViewString(Project project)
        {
            return project.AssemblyVersion
                .Match(x =>
                        $"{project.Name} - {VersionConverter.ToString(project.Version)} - {VersionConverter.ToString(x)}",
                    $"{project.Name} - {VersionConverter.ToString(project.Version)}");
        }
    }
}