using System;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.ProjectManipulator;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests.ProjectManipulator
{
    public class NodeModifierTests
    {
        [Fact]
        public void Set_ChangeVersion_ShouldChangeToModifyProject()
        {
            var testProject = TestPathHelper.GetTestProjectPath("TO_MODIFY");

            var newPatchVersion = Convert.ToInt32((DateTime.Now - DateTime.Today).TotalSeconds);
            var setVersion = $"1.0.{newPatchVersion}";
            NodeModifier.Set(testProject, VersionNode.Version, setVersion);
            
            var version = NodeFinder.GetVersionNodeValue(testProject, VersionNode.Version);
            version.ValueUnsafe().ShouldBe(setVersion);
        }
        
        [Fact]
        public void Set_ChangeAssemblyVersion_ShouldChangeToModifyProject()
        {
            var testProject = TestPathHelper.GetTestProjectPath("TO_MODIFY");

            var newBuildNumber = Convert.ToInt32((DateTime.Now - DateTime.Today).TotalSeconds);
            var setAssemblyVersion = $"1.0.{newBuildNumber}";
            NodeModifier.Set(testProject, VersionNode.AssemblyVersion, setAssemblyVersion);
            
            var assemblyVersion = NodeFinder.GetVersionNodeValue(testProject, VersionNode.AssemblyVersion);
            assemblyVersion.ValueUnsafe().ShouldBe(setAssemblyVersion);
        }
    }
}