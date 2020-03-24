using Paczker.Domain.Model;

namespace Paczker.Core.VersionOperators
{
    public static class VersionConverter
    {
        public static string ToString(Version version)
        {
            return $"{string.Join('.', version.Major, version.Minor, version.Patch)}{version.Label}";
        }

        public static string ToString(AssemblyVersion assemblyVersionPart)  =>
            string.Join('.', assemblyVersionPart.Major, assemblyVersionPart.Minor, assemblyVersionPart.BuildNumber,
                assemblyVersionPart.Revision);
        }
}