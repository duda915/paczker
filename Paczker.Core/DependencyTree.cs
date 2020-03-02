using System;
using System.Collections.Generic;
using System.Linq;

namespace Paczker.Core
{
    public static class DependencyTree
    {
        public static IEnumerable<Project> FindReferences(IEnumerable<Project> projects, string projectName)
        {
            var refs = FindReferencesRec(projects, projectName);
            return refs.Append(projects.First(x => x.Name.Equals(projectName)));
        }
        
        public static IEnumerable<Project> FindReferencesRec(IEnumerable<Project> projects, string projectName)
        {
            var referencedBy = projects.Where(x =>
                x.Dependencies.Any(y => string.Equals(y.Name, projectName, StringComparison.OrdinalIgnoreCase)));

            return referencedBy.Any()
                ? referencedBy.Append(referencedBy.SelectMany(x => FindReferences(projects, x.Name))).Distinct(new ProjectNameComparer())
                : referencedBy;
        }
        
        public class ProjectNameComparer : IEqualityComparer<Project>
        {
            public bool Equals(Project x, Project y)
            {
                return string.Equals(x?.Name, y?.Name);
            }

            public int GetHashCode(Project obj)
            {
                return string.GetHashCode(obj.Name);
            }
        }
    }
}