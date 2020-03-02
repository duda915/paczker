using System;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace Paczker.Core
{
    public static class VersionConverter
    {
        public static string ToCsProjFormat(Version version)
        {
            var versionTriplet = string.Join('.', version.Major, version.Minor, version.Patch);
            var finalVersion = string.IsNullOrWhiteSpace(version.Postfix)
                ? versionTriplet
                : string.Join('-', versionTriplet, version.Postfix);
            
            return finalVersion;
        }

        public static Version ToVersion(string version)
        {
            var versionSplits = version.Split('.');
            var major = versionSplits[0];
            var minor = versionSplits[1];
            
            var patchSplits = versionSplits.Skip(2)
                .Reduce((x, y) => x + y);

            var patch = string.Concat(patchSplits.TakeWhile(char.IsDigit));
            var postfix = patchSplits.TrimStart(patch.Append("-").ToArray());

            return new Version(major, minor, patch, postfix);
        }

        public static string ToCsProjFormat(AssemblyVersion assemblyVersion)  =>
            string.Join('.', assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.BuildNumber,
                assemblyVersion.Revision);

        public static AssemblyVersion ToAssemblyVersion(string assemblyVersion)
        {
            var versionSplits = assemblyVersion.Split('.').Select(x => Convert.ToInt32(x)).ToArr();
            var major = versionSplits.ElementAtOrDefault(0).ToString();
            var minor = versionSplits.ElementAtOrDefault(1).ToString();
            var buildNumber = versionSplits.ElementAtOrDefault(2).ToString();
            var revision = versionSplits.ElementAtOrDefault(3).ToString();

            return new AssemblyVersion
            {
                Major = major,
                Minor = minor,
                BuildNumber = buildNumber,
                Revision = revision
            };
        }

        public enum VersionPart
        {
            Major, Minor, Patch
        }
        
        public static class VersionModifier
        {
            public static Project IncrementVersion(Project project, VersionPart part)
            {
                var version = ToVersion(project.Version);
                var assemblyVersion = project.AssemblyVersion.Map(ToAssemblyVersion);

                (string versionPart, Option<string> assemblyVersionPart) Inc(Func<Version, string> versionSelector, Func<AssemblyVersion, string> assemblyVersionSelector)
                {
                    var newVersion = (Convert.ToInt32(versionSelector(version)) + 1).ToString();
                    var newAssembly = assemblyVersion.Map(x => (Convert.ToInt32(assemblyVersionSelector(x)) + 1).ToString());

                    return (newVersion, newAssembly);
                }

                switch (part)
                {
                    case VersionPart.Major:
                    {
                        var newMajors = Inc(x => x.Major, x => x.Major);
                        project.Version = ToCsProjFormat(new Version(newMajors.versionPart, version.Minor, version.Patch, version.Postfix));
                        project.AssemblyVersion = assemblyVersion.Map(x =>
                            ToCsProjFormat(new AssemblyVersion(newMajors.assemblyVersionPart.ValueUnsafe(), x.Minor, x.BuildNumber, x.Revision)));
                        break;
                    }
                    case VersionPart.Minor:
                    {
                        var newMajors = Inc(x => x.Minor, x => x.Minor);
                        project.Version = ToCsProjFormat(new Version(version.Major, newMajors.versionPart, version.Patch, version.Postfix));
                        project.AssemblyVersion = assemblyVersion.Map(x =>
                            ToCsProjFormat(new AssemblyVersion(x.Major, newMajors.assemblyVersionPart.ValueUnsafe(), x.BuildNumber, x.Revision)));
                        break;
                    }
                    case VersionPart.Patch:
                    {
                        var newMajors = Inc(x => x.Patch, x => x.BuildNumber);
                        project.Version = ToCsProjFormat(new Version(version.Major, version.Minor, newMajors.versionPart, version.Postfix));
                        project.AssemblyVersion = assemblyVersion.Map(x =>
                            ToCsProjFormat(new AssemblyVersion(x.Major, x.Minor, newMajors.assemblyVersionPart.ValueUnsafe(), x.Revision)));
                        break;
                    }
                }

                return project;
            }
            
            public static Project SetPreVersion(Project project)
            {
                var version = ToVersion(project.Version);
                var assemblyVersion = project.AssemblyVersion.Map(ToAssemblyVersion);
                var newVersion = ToCsProjFormat(new Version(version.Major, version.Minor, version.Patch, $"pre{DateTime.UtcNow.Ticks}"));
                var newAssemblyVersion = assemblyVersion.Map(x =>
                {
                    var revInt = Convert.ToInt32(x.Revision);
                    if (revInt < 1000) revInt = 999;
                    x.Revision = (revInt + 1).ToString();
                    return ToCsProjFormat(x);
                });

                project.Version = newVersion;
                project.AssemblyVersion = newAssemblyVersion;
                return project;
            }
            
            public static Project RemovePreVersion(Project project)
            {
                var version = ToVersion(project.Version);
                var assemblyVersion = project.AssemblyVersion.Map(ToAssemblyVersion);
                var newVersion = ToCsProjFormat(new Version(version.Major, version.Minor, version.Patch, string.Empty));
                var newAssemblyVersion = assemblyVersion.Map(x =>
                {
                    x.Revision = 0.ToString();
                    return ToCsProjFormat(x);
                });
                
                project.Version = newVersion;
                project.AssemblyVersion = newAssemblyVersion;
                return project;
            }
        }
    }
}