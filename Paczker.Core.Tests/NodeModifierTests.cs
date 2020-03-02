using Paczker.Core.ProjectManipulator;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests
{
    public class NodeModifierTests
    {
        [Fact]
        public void Set_Version_ShouldChangeVersion()
        {
            var projectA = TestPathHelper.GetTestProjectPath("A");
            
            var doc = ProjectLoader.Load(projectA);
            NodeFinder.GetVersionNodeValue(doc, VersionNode.Version).ShouldBe("1.0.0");
            NodeModifier.Set(doc, VersionNode.Version, "1.1.1");

            var doc1 = ProjectLoader.Load(projectA);
            NodeFinder.GetVersionNodeValue(doc1, VersionNode.Version).ShouldBe("1.1.1");
            NodeModifier.Set(doc1, VersionNode.Version, "1.0.0");

            var doc2 = ProjectLoader.Load(projectA);
            NodeFinder.GetVersionNodeValue(doc2, VersionNode.Version).ShouldBe("1.0.0");
        }
    }
}