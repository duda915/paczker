using System.Collections.Generic;
using LanguageExt.Common;
using Paczker.Core.Commands;
using Xunit;

namespace Paczker.Core.Tests
{
    public class ListDependentProjectsCommandTests
    {
        [Fact]
        public void ListDependentProjects_ForProjectWithFourDependencies_ListAll()
        {
            var cProject = TestPathHelper.GetTestSolutionPath();
            ListDependentProjectsCommand.ListDependentProjects(cProject, "C_AB.csproj");
        }

    }
}