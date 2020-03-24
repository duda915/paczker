using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;

namespace Paczker.Core.ProjectManipulator
{
    public class ProjectSaver
    {
        public static Project Save(Project project)
        {
            NodeModifier.Set(project.Path, VersionNode.Version, VersionConverter.ToString(project.Version));
            project.AssemblyVersion.IfSome(x => 
                NodeModifier.Set(project.Path, VersionNode.AssemblyVersion, VersionConverter.ToString(x)));
            
            return project;
        }
    }
}