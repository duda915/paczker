using System.Linq;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.VersionOperators;
using Shouldly;
using Xunit;

namespace Paczker.Core.Tests.VersionOperators
{
    public class VersionFactoryTests
    {
        [Fact]
        public void CreateVersion_ForValidVersionWithoutLabel_ReturnVersionWithEmptyLabel()
        {
            var versionString = "1.2.3";

            var version = VersionFactory.CreateVersion(versionString).ValueUnsafe();
            
            version.Major.ShouldBe(1);
            version.Minor.ShouldBe(2);
            version.Patch.ShouldBe(3);
            version.Label.ShouldBeEmpty();
        }
        
        [Fact]
        public void CreateVersion_ForValidVersionWithLabel_ReturnVersionWithLabel()
        {
            var versionString = "1.2.3-pre12";

            var version = VersionFactory.CreateVersion(versionString).ValueUnsafe();
            
            version.Major.ShouldBe(1);
            version.Minor.ShouldBe(2);
            version.Patch.ShouldBe(3);
            version.Label.ShouldBe("-pre12");
        }
        
        [Fact]
        public void CreateVersion_ForInvalidVersion_ReturnNone()
        {
            var versionString = "invalid";

            var version = VersionFactory.CreateVersion(versionString);
            
            version.IsNone.ShouldBeTrue();
        }

        [Fact]
        public void CreateVersion_ForInvalidVersionsWithGoodFormat_ReturnNone()
        {
            var invalidVersions = new[]
            {
                "a.a.a", "1.a.b", "a.2.a", "a.a.3"
            };
            
            invalidVersions.Select(VersionFactory.CreateVersion)
                .ShouldAllBe(x => x.IsNone);
        }
        
        [Fact]
        public void CreateAssemblyVersion_ForValidVersion_ReturnAssemblyVersion()
        {
            var versionString = "1.2.3.4";

            var version = VersionFactory.CreateAssemblyVersion(versionString).ValueUnsafe();
            
            version.Major.ShouldBe(1);
            version.Minor.ShouldBe(2);
            version.BuildNumber.ShouldBe(3);
            version.Revision.ShouldBe(4);
        }
        
        [Fact]
        public void CreateAssemblyVersion_ForValidVersionWithoutRevision_ReturnAssemblyVersionWithZeroRevision()
        {
            var versionString = "1.2.3";

            var version = VersionFactory.CreateAssemblyVersion(versionString).ValueUnsafe();
            
            version.Major.ShouldBe(1);
            version.Minor.ShouldBe(2);
            version.BuildNumber.ShouldBe(3);
            version.Revision.ShouldBe(0);
        }
        
        [Fact]
        public void CreateAssemblyVersion_ForInvalidAssemblyVersion_ReturnNone()
        {
            var versionString = "invalid";

            var version = VersionFactory.CreateAssemblyVersion(versionString);
            
            version.IsNone.ShouldBeTrue();
        }

        [Fact]
        public void CreateAssemblyVersion_ForInvalidAssemblyVersionsWithGoodFormat_ReturnNone()
        {
            var invalidVersions = new[]
            {
                "a.a.a", "1.a.b", "a.2.a", "a.a.3", "a.a.a.4"
            };
            
            invalidVersions.Select(VersionFactory.CreateAssemblyVersion)
                .ShouldAllBe(x => x.IsNone);
        }
    }
}