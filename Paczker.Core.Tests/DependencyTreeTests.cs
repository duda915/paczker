using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.SolutionDiscovery;
using Shouldly;
using Xunit;
using static Paczker.Core.Tests.TestPathHelper;
using static Paczker.Core.DependencyTree;
using static LanguageExt.Prelude;
namespace Paczker.Core.Tests
{
    public class DependencyTreeTests
    {
        [Fact]
        public void FindReferences_ForProjectReferencedByZeroProjects_ReturnEmptyList()
        {
            var path = GetTestSolutionPath();
            var references = ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => FindReferences(x, "F-A.fsproj"));

            references.IsSuccess.ShouldBeTrue();
            references.IfFail(ImmutableArray<Project>.Empty).ShouldBeEmpty();
        }
        
        [Fact]
        public void FindReferences_ForProjectReferencedByFiveProjects_ReturnFiveProjects()
        {
            var path = GetTestSolutionPath();
            var references = ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => FindReferences(x, "A.csproj"));

            references.IsSuccess.ShouldBeTrue();
            var result = references.IfFail(ImmutableArray<Project>.Empty).OrderBy(x => x.Name).ToList();
            result.Count().ShouldBe(5);
            result[0].Name.ShouldBe("C_AB.csproj");
            result[1].Name.ShouldBe("D_C.csproj");
            result[2].Name.ShouldBe("E_BC.csproj");
            result[3].Name.ShouldBe("F_A.fsproj");
            result[4].Name.ShouldBe("G_CE.csproj");
        }
    }
}