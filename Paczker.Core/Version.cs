using LanguageExt;
using static LanguageExt.Prelude;
namespace Paczker.Core
{
    public class Version
    {
        public readonly string Major;
        public readonly string Minor;
        public readonly string Patch;
        public readonly string Postfix;

        public Version(string major, string minor, string patch, string postfix)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Postfix = postfix;
        }
    }

    public class AssemblyVersion
    {
        public string Major { get; set; }
        public string Minor { get; set; }
        public string BuildNumber { get; set; }
        public string Revision { get; set; }

        public AssemblyVersion()
        {
        }

        public AssemblyVersion(string major, string minor, string buildNumber, string revision)
        {
            Major = major;
            Minor = minor;
            BuildNumber = buildNumber;
            Revision = revision;
        }
    }
}