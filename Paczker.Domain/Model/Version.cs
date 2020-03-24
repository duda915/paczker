using LanguageExt;

namespace Paczker.Domain.Model
{
    [WithLens]
    public partial class Version : Record<Version>
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;
        public readonly string Label;

        public Version(int major, int minor, int patch, string label)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Label = label;
        }
    }
}