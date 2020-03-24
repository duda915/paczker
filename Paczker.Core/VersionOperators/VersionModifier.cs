using System;
using LanguageExt;
using Paczker.Domain.Model;
using static LanguageExt.Prelude;
using Version = Paczker.Domain.Model.Version;

namespace Paczker.Core.VersionOperators
{
    public static class VersionModifier
    {
        public static Project VersionOperator(Project project, VersionPart part, Func<int, int> operation)
        {
            var lens = VersionLens(part);
            return lens.Set(operation(lens.Get(project)), project);
        }
        
        public static Project AssemblyVersionOperator(Project project, AssemblyVersionPart part, Func<int, int> operation)
        {
            var lens = AssemblyVersionLens(part);
            return project.AssemblyVersion.Match(
                x => project.With(AssemblyVersion: lens.Set(operation(lens.Get(x)), x)), project);
        }

        private static Lens<Project, int> VersionLens(VersionPart versionPart)
        {
            return versionPart switch
            {
                VersionPart.Major => lens(Project.version, Version.major),
                VersionPart.Minor => lens(Project.version, Version.minor),
                VersionPart.Patch => lens(Project.version, Version.patch),
                _ => lens(Project.version, Version.major)
            };
        }
        
        private static Lens<AssemblyVersion, int> AssemblyVersionLens(AssemblyVersionPart assemblyVersionPart)
        {
            return assemblyVersionPart switch
            {
                AssemblyVersionPart.Major => AssemblyVersion.major,
                AssemblyVersionPart.Minor => AssemblyVersion.minor,
                AssemblyVersionPart.BuildNumber => AssemblyVersion.buildNumber,
                AssemblyVersionPart.Revision => AssemblyVersion.revision,
                _ => AssemblyVersion.major
            };
        }

        public static Project SetPreReleaseVersion(Project project)
        {
            Func<Project, Project> SetVersion = x =>
                x.With(Version: x.Version.With(Label: $"-pre{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}"));
            Func<Project, Project> SetAssemblyVersion = x =>
                AssemblyVersionOperator(x, AssemblyVersionPart.Revision, y => y < 1000 ? 1000 : y + 1);
            
            return compose(SetAssemblyVersion, SetVersion)(project);
        }

        public static Project RemovePreReleaseVersion(Project project)
        {
            Func<Project, Project> RemoveVersion = x => x.With(Version: x.Version.With(Label: string.Empty));
            Func<Project, Project> RemoveAssemblyVersion =
                x => AssemblyVersionOperator(x, AssemblyVersionPart.Revision, _ => 0);

            return compose(RemoveVersion, RemoveAssemblyVersion)(project);
        }

        public static Project IncrementVersion(Project project, VersionPart versionPart)
        {
            var assemblyVersionPart = (AssemblyVersionPart) (int) versionPart;
            Func<Project, Project> IncrementVersion = x => VersionOperator(x, versionPart, y => y + 1);
            Func<Project, Project> IncrementAssemblyVersion =
                x => AssemblyVersionOperator(x, assemblyVersionPart, y => y + 1);
            
            return compose(IncrementVersion, IncrementAssemblyVersion)(project);
        }
        
        public static Project DecrementVersion(Project project, VersionPart versionPart)
        {
            var assemblyVersionPart = (AssemblyVersionPart) (int) versionPart;
            Func<Project, Project> DecrementVersion = x => VersionOperator(x, versionPart, y => y == 0 ? 0 : y - 1);
            Func<Project, Project> DecrementAssemblyVersion =
                x => AssemblyVersionOperator(x, assemblyVersionPart, y => y == 0 ? 0 : y - 1);
            
            return compose(DecrementVersion, DecrementAssemblyVersion)(project);
        }
    }
}