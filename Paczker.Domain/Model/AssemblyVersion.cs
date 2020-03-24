using LanguageExt;

namespace Paczker.Domain.Model
{
    [WithLens]
    public partial class AssemblyVersion : Record<AssemblyVersion>
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int BuildNumber;
        public readonly int Revision;

        public AssemblyVersion(int major, int minor, int buildNumber, int revision)
        {
            Major = major;
            Minor = minor;
            BuildNumber = buildNumber;
            Revision = revision;
        }
    }
}