using Shouldly;
using Xunit;
using static Paczker.Core.VersionConverter;
namespace Paczker.Core.Tests
{
    public class VersionConverterTests
    {
        [Fact]
        public void ToVersion_NormalFormat_ShouldConvertToVersion()
        {
            var result = ToVersion("1.2.3");
            
            result.Major.ShouldBe("1");
            result.Minor.ShouldBe("2");
            result.Patch.ShouldBe("3");
            result.Postfix.ShouldBeEmpty();
        }

        [Fact]
        public void ToVersion_WithPostfix_ShouldConvertToVersionWithPostfix()
        {
            var result = ToVersion("1.2.3-pre666");
            
            result.Major.ShouldBe("1");
            result.Minor.ShouldBe("2");
            result.Patch.ShouldBe("3");
            result.Postfix.ShouldBe("pre666");
        }

        [Fact]
        public void ToAssemblyVersion_WhenAssemblyContainsThreeNumbers_SetLastWithZero()
        {
            var assemblyVersion = "1.2.2";
            
            var result = ToAssemblyVersion(assemblyVersion);
            result.Major.ShouldBe("1");
            result.Minor.ShouldBe("2");
            result.BuildNumber.ShouldBe("2");
            result.Revision.ShouldBe("0");
        }
        
        [Fact]
        public void ToAssemblyVersion_WhenAssemblyContainsFourNumbers_MapAllNumbers()
        {
            var assemblyVersion = "1.2.2.4";
            
            var result = ToAssemblyVersion(assemblyVersion);
            result.Major.ShouldBe("1");
            result.Minor.ShouldBe("2");
            result.BuildNumber.ShouldBe("2");
            result.Revision.ShouldBe("4");
        }
        
        [Fact]
        public void ToCsProjFormat_WhenPostfixEmpty_IgnorePostfix()
        {
            var version = new Version("1", "2", "3", string.Empty);
            
            var result = ToCsProjFormat(version);
            result.ShouldBe("1.2.3");
        }
        
        [Fact]
        public void ToCsProjFormat_WhenPostfixNotEmpty_AppendToVersion()
        {
            var version = new Version("1", "2", "3", "sx");
            
            var result = ToCsProjFormat(version);
            result.ShouldBe("1.2.3-sx");
        }
        
    }
}