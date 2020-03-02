using System;
using System.Linq;
using Paczker.Core.SolutionDiscovery;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests
{
    public class ProjectsScannerTests
    {
        [Fact]
        public void GetAllProjectsInDirectory_ForTestSolution_ReturnFourStringPaths()
        {
            var projectsResult = ProjectsScanner.GetAllProjectsInDirectory($@"{Environment.CurrentDirectory}/TestSolution/");

            projectsResult.IsSuccess.ShouldBeTrue();
            var projects = projectsResult.Match(x => x, _ => new string[0]).OrderBy(x => x).ToArray();
            
            projects[0].ShouldEndWith("A.csproj");
            projects[1].ShouldEndWith("B.csproj");
            projects[2].ShouldEndWith("C_AB.csproj");
            projects[3].ShouldEndWith("D_C.csproj");
            projects[4].ShouldEndWith("E_BC.csproj");
            projects[5].ShouldEndWith("F_A.fsproj");
        }
    }
}