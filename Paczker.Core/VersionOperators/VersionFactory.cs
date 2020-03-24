using System;
using System.Linq;
using LanguageExt;
using Paczker.Domain.Model;
using static LanguageExt.Prelude;
using Version = Paczker.Domain.Model.Version;

namespace Paczker.Core.VersionOperators
{
    public static class VersionFactory
    {
        public static Option<AssemblyVersion> CreateAssemblyVersion(string assemblyVersion)
        {
            var versionSplits = assemblyVersion.Split('.')
                .Select(x => Prelude.TryOption(() => Convert.ToInt32(x))()).ToList();

            if (versionSplits.Any(x => x.IsFaultedOrNone))
                return Prelude.None;

            var major = versionSplits.ElementAtOrDefault(0).IfFailOrNone(0);
            var minor = versionSplits.ElementAtOrDefault(1).IfFailOrNone(0);
            var buildNumber = versionSplits.ElementAtOrDefault(2).IfFailOrNone(0);
            var revision = versionSplits.ElementAtOrDefault(3).IfFailOrNone(0);

            return new AssemblyVersion(major, minor, buildNumber, revision);
        }
        
        public static Option<Version> CreateVersion(string version)
        {
            var versionSplits = version.Split('.');

            if (versionSplits.Length != 3)
                return None;

            var major = TryOption(() => versionSplits.ElementAtOrDefault(0))
                .Bind(x => TryOption(() => Convert.ToInt32(x)));
            var minor = TryOption(() => versionSplits.ElementAtOrDefault(1))
                .Bind(x => TryOption(() => Convert.ToInt32(x)));
            
            var patchSplits = versionSplits.Skip(2)
                .Reduce((x, y) => x + y);

            var patch = TryOption(() => Convert.ToInt32(string.Concat(patchSplits.TakeWhile(char.IsDigit))));
            
            var postfix = patchSplits.TrimStart(patch.ToString().ToArray());

            var versionTryOption = 
                from mj in major
                from mi in minor
                from p in patch
                select new Version(mj, mi, p, postfix);

            return versionTryOption.ToOption();
        }
    }
}