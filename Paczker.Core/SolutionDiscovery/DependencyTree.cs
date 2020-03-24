using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Paczker.Core.ProjectManipulator;
using Paczker.Domain.Model;
using static LanguageExt.Prelude;

namespace Paczker.Core.SolutionDiscovery
{
    public static class DependencyTree
    {
        public static IEnumerable<Project> FindReferences(IEnumerable<Project> projects, IEnumerable<string> projectNames)
        {
            return projectNames.SelectMany(x => FindReferences(projects, x)).Distinct();
        }

        public static IEnumerable<Project> FindReferences(IEnumerable<Project> projects, string projectName)
        {
            var projectsList = projects.ToList();
            var project = projectsList.FirstOrDefault(x => x.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
            
            return project == default ? Enumerable.Empty<Project>() : FindReferencesRec(projectsList, project).Append(project).ToList();
        }
        
        private static IEnumerable<Project> FindReferencesRec(IEnumerable<Project> projects, Project project)
        {
            var projectsList = projects.ToList();
            Func<string, IEnumerable<string>> getDependenciesPaths = NodeFinder.GetProjectReferencePaths;
            Func<IEnumerable<string>, IEnumerable<Option<Project>>> mapPathsToProjects = x =>
                x.Map(y => ProjectsFactory.MapCsProj(ProjectsScanner.GetProjectNameFromPath(y), y));
            var getProjectDependencies = compose(getDependenciesPaths, mapPathsToProjects);
            
            var dependenciesMap = projectsList.ToDictionary(x => x, x => getProjectDependencies(x.Path));

            var referencedBy = dependenciesMap.Where(x => x.Value.Any(y => y.Match(z => z == project, false)))
                .Select(x => x.Key).ToList();

            return referencedBy.Any()
                ? referencedBy.ConcatFast(referencedBy.SelectMany(x => FindReferencesRec(projectsList, x))).Distinct()
                : Enumerable.Empty<Project>();
        }
    }
}