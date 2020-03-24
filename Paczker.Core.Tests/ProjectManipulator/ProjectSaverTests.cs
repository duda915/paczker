using System;
using LanguageExt;
using Paczker.Core.ProjectManipulator;
using Paczker.Domain.Model;
using Shouldly;
using Xunit;
using Version = Paczker.Domain.Model.Version;

namespace Paczker.Core.Tests.ProjectManipulator
{
    public class ProjectSaverTests
    {
        [Fact]
        public void Save_ProjectWithAssemblyVersion_ShouldSaveBothVersions()
        {
            var testProject = TestPathHelper.GetTestProjectPath("TO_MODIFY");
            var newPatch = Convert.ToInt32((DateTime.Now - DateTime.Today).TotalSeconds);
            var project = new Project("TO_MODIFY", new Version(1, 1, newPatch, ""),
                new AssemblyVersion(1, 1, newPatch, 0), testProject);

            ProjectSaver.Save(project);

            var savedVersion = $"1.1.{newPatch}";
            var savedAssemblyVersion = $"1.1.{newPatch}.0";
            NodeFinder.GetVersionNodeValue(testProject, VersionNode.Version).ShouldBe(savedVersion);
            NodeFinder.GetVersionNodeValue(testProject, VersionNode.AssemblyVersion).ShouldBe(savedAssemblyVersion);
        }
        
        [Fact]
        public void Save_ProjectWithoutAssemblyVersion_ShouldSaveVersionOnly()
        {
            var testProject = TestPathHelper.GetTestProjectPath("TO_MODIFY");
            var newPatch = Convert.ToInt32((DateTime.Now - DateTime.Today).TotalSeconds);
            var project = new Project("TO_MODIFY", new Version(1, 1, newPatch, ""),
                Option<AssemblyVersion>.None, testProject);

            ProjectSaver.Save(project);

            var savedVersion = $"1.1.{newPatch}";
            var notSavedAssemblyVersion = $"1.1.{newPatch}.0";
            NodeFinder.GetVersionNodeValue(testProject, VersionNode.Version).ShouldBe(savedVersion);
            NodeFinder.GetVersionNodeValue(testProject, VersionNode.AssemblyVersion).ShouldNotBe(notSavedAssemblyVersion);
        }
    }
}