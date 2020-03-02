using System;
using System.Linq;
using LanguageExt.UnsafeValueAccess;
using Paczker.Core.ProjectManipulator;
using Paczker.Core.SolutionDiscovery;
using Paczker.Infrastructure;

namespace Paczker.Core.Commands
{
    public class IncrementProjectsCommand
    {
        public static void IncrementProjects(string path, string projectName, string versionPart)
        {
            var part = Enum.Parse<VersionConverter.VersionPart>(versionPart);
            
            LoggerFactory.LogInfo($"Incrementing {projectName}");
            ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Map(ProjectsMapper.MapCsProj))
                .Map(x => x.Filter(y => y.IsSome).Map(y => y.ValueUnsafe()))
                .Map(x => DependencyTree.FindReferences(x, projectName))
                .Map(x => x.Map(y => VersionConverter.VersionModifier.IncrementVersion(y, part)).ToList())
                .Map(x =>
                {
                    x.Iter(y =>
                    {
                        NodeModifier.Set(y.Path, VersionNode.Version, y.Version);
                        y.AssemblyVersion.Iter(
                            z => { NodeModifier.Set(y.Path, VersionNode.AssemblyVersion, z); });
                    });

                    return x;
                }).Map(x => x.Iter(y => LoggerFactory.LogInfo($"{y.Name} -> {y.Version} || {y.AssemblyVersion.IfNone("")}")));
        }
        
        public static void SetPre(string path, string projectName)
        {
            LoggerFactory.LogInfo($"Setting development version for {projectName}");
            var result = ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => DependencyTree.FindReferences(x, projectName))
                .Map(x => x.Select(VersionConverter.VersionModifier.SetPreVersion).ToList())
                .Map(x =>
                {
                    x.Iter(y =>
                    {
                        NodeModifier.Set(y.Path, VersionNode.Version, y.Version);
                        y.AssemblyVersion.Iter(
                            z => { NodeModifier.Set(y.Path, VersionNode.AssemblyVersion, z); });
                    });
                    
                    return x;
                });

            result.Map(x => x.Iter(y => LoggerFactory.LogInfo($"{y.Name} -> {y.Version} || {y.AssemblyVersion.IfNone("")}")));

        }
        
        public static void RemovePre(string path, string projectName)
        {
            LoggerFactory.LogInfo($"Removing development version from {projectName}");
            var result = ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => DependencyTree.FindReferences(x, projectName))
                .Map(x => x.Select(VersionConverter.VersionModifier.RemovePreVersion).ToList())
                .Map(x =>
                {
                    x.Iter(y =>
                    {
                        NodeModifier.Set(y.Path, VersionNode.Version, y.Version);
                        y.AssemblyVersion.Iter(
                            z => { NodeModifier.Set(y.Path, VersionNode.AssemblyVersion, z); });
                    });
                    
                    return x;
                });

            result.Map(x => x.Iter(y => LoggerFactory.LogInfo($"{y.Name} -> {y.Version} || {y.AssemblyVersion.IfNone("")}")));

        }
        
        public static void Push(string path, string projectName, string source, string buildProfile)
        {
            LoggerFactory.LogInfo($"Pushing {projectName} to {source}");
            
            Pusher.Build(path, buildProfile);

            ProjectsScanner.GetAllProjectsInDirectory(path)
                .Map(x => x.Select(ProjectsMapper.MapCsProj))
                .Map(x => x.Where(y => y.IsSome).Select(y => y.ValueUnsafe()))
                .Map(x => DependencyTree.FindReferences(x, projectName))
                .Map(x =>
                {
                    x.Iter(y => Pusher.PushToSource(y, source, buildProfile));
                    return x;
                }).Map(x => x.Iter(y => LoggerFactory.LogInfo($"Pushed {y.Name} {y.Version}")));
        }
    }
}