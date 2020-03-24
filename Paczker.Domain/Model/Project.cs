using LanguageExt;

namespace Paczker.Domain.Model
{
    [WithLens]
    public partial class Project : Record<Project>
    {
        public readonly string Name;
        public readonly Version Version;
        public readonly Option<AssemblyVersion> AssemblyVersion;
        public readonly string Path;

        public Project(string name, Version version, Option<AssemblyVersion> assemblyVersion, string path)
        {
            Name = name;
            Version = version;
            AssemblyVersion = assemblyVersion;
            Path = path;
        }
    }
}