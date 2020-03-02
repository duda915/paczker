using System;
using System.IO;
using System.Linq;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.SolutionDiscovery;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests
{
    public class ProjectsMapperTests
    {
        [Fact]
        public void MapCsProj_ForProjectWithNoDependencies_MapProjectWithEmptyDependencies()
        {
            var projectsResult = ProjectsMapper.MapCsProj(TestPathHelper.GetTestProjectPath("A"));
            
            projectsResult.IsSome.ShouldBeTrue();
            var project = projectsResult.ValueUnsafe();
            project.Name.ShouldBe("A.csproj");
            project.Dependencies.ShouldBeEmpty();
            project.Version.ShouldBe("1.0.0");
            project.AssemblyVersion.ShouldBe("1.0.0");
            project.Path.ShouldBe(TestPathHelper.GetTestProjectPath("A"));
        }

        [Fact]
        public void MapCsProj_ForProjectWithTwoNotNestedDependencies_MapProjectWithTwoDependencies()
        {
            var projectsResult = ProjectsMapper.MapCsProj(TestPathHelper.GetTestProjectPath("C_AB"));
            
            projectsResult.IsSome.ShouldBeTrue();
            var project = projectsResult.ValueUnsafe();
            project.Name.ShouldBe("C_AB.csproj");
            project.Dependencies.Length().ShouldBe(2);
            project.Version.ShouldBe("1.0.0");
            project.AssemblyVersion.ShouldBe("1.0.0");
            project.Path.ShouldBe(TestPathHelper.GetTestProjectPath("C_AB"));

            var dependencies = project.Dependencies.ToList();
            dependencies.Count.ShouldBe(2);

            var dependencyA = dependencies[0];
            dependencyA.Dependencies.ShouldBeEmpty();
            dependencyA.Version.ShouldBe("1.0.0");
            dependencyA.AssemblyVersion.ShouldBe("1.0.0");
            IsTheSamePath(dependencyA.Path, TestPathHelper.GetTestProjectPath("A")).ShouldBeTrue();

            var dependencyB = dependencies[1];
            dependencyB.Dependencies.ShouldBeEmpty();
            dependencyB.Version.ShouldBe("1.0.0");
            dependencyB.AssemblyVersion.ShouldBe("1.0.0");
            IsTheSamePath(dependencyB.Path, TestPathHelper.GetTestProjectPath("B")).ShouldBeTrue();
        }
        
        [Fact]
        public void MapCsProj_ForProjectWithThreeLevels_DependenciesShouldBeProperlyMapped()
        {
            var projectsResult = ProjectsMapper.MapCsProj(TestPathHelper.GetTestProjectPath("D_C"));
            
            projectsResult.IsSome.ShouldBeTrue();
            var firstLevelProject = projectsResult.ValueUnsafe();
            var firstLevelProjectDependencies = firstLevelProject.Dependencies.ToList();
            firstLevelProjectDependencies.Count.ShouldBe(1);
            var secondLevelProjectDependencies = firstLevelProjectDependencies[0].Dependencies.ToList();
            secondLevelProjectDependencies.Count.ShouldBe(2);
        }

        private bool IsTheSamePath(string pathA, string pathB)
        {
            return Path.GetFullPath(pathA).Equals(Path.GetFullPath(pathB), StringComparison.OrdinalIgnoreCase);
        }
    }
}