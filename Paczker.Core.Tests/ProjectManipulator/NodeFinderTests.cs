using System.Linq;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.ProjectManipulator;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests.ProjectManipulator
{
    public class NodeFinderTests
    {
        [Fact]
        public void GetVersionNodeValue_ForVersion_ReturnVersionString()
        {
            var testProj = TestPathHelper.GetTestProjectPath("A");
            var version = NodeFinder.GetVersionNodeValue(testProj, VersionNode.Version);
            
            version.ValueUnsafe().ShouldBe("1.0.0");
        }
        
        [Fact]
        public void GetVersionNodeValue_ForAssemblyVersion_ReturnAssemblyVersionString()
        {
            var testProj = TestPathHelper.GetTestProjectPath("A");
            var version = NodeFinder.GetVersionNodeValue(testProj, VersionNode.AssemblyVersion);
            
            version.ValueUnsafe().ShouldBe("1.0.1");
        }
        
        [Fact]
        public void GetVersionNodeValue_WhenProjectNotPackageProject_ReturnNone()
        {
            var testProj = TestPathHelper.GetTestProjectPath("H");
            var version = NodeFinder.GetVersionNodeValue(testProj, VersionNode.Version);
            var assemblyVersion = NodeFinder.GetVersionNodeValue(testProj, VersionNode.AssemblyVersion);
            
            version.IsNone.ShouldBeTrue();
            assemblyVersion.IsNone.ShouldBeTrue();
        }
        
        [Fact]
        public void GetProjectReferencePaths_WhenProjectHasNoReferences_ReturnEmptyList()
        {
            var testProj = TestPathHelper.GetTestProjectPath("H");
            var references = NodeFinder.GetProjectReferencePaths(testProj);

            references.ShouldBeEmpty();
        }
        
        [Fact]
        public void GetProjectReferencePaths_WhenProjectHasTwoReferences_ReturnListWithTwoReferences()
        {
            var testProj = TestPathHelper.GetTestProjectPath("C_AB");
            var references = NodeFinder.GetProjectReferencePaths(testProj).ToList();
            references[0].Contains("A.csproj").ShouldBeTrue();
            references[1].Contains("B.csproj").ShouldBeTrue();
        }
    }
}