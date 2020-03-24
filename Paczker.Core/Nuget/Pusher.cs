using System.Diagnostics;
using System.IO;
using Paczker.Core.SolutionDiscovery;
using Paczker.Core.VersionOperators;
using Paczker.Domain.Model;

namespace Paczker.Core.Nuget
{
    public class Pusher
    {
        public static string PushToSource(Project project, string source, string buildProfile)
        {
            var pathToPackage = Path.Combine(Path.GetDirectoryName(project.Path), "bin", buildProfile,
                $"{ProjectsScanner.GetProjectNameFromPath(project.Path)}.{VersionConverter.ToString(project.Version)}.nupkg");

            using var cmd = GetCommandProcess("dotnet", $"nuget push --source {source} {pathToPackage}");
            
            cmd.Start();
            cmd.WaitForExit();
            
            return cmd.StandardOutput.ReadToEnd();
        }
        
        public static string Build(Project project, string buildProfile)
        {
            using var cmd = GetCommandProcess("dotnet", $"pack -c {buildProfile} {project.Path}");
            
            cmd.Start();
            cmd.WaitForExit();

            return cmd.StandardOutput.ReadToEnd();
        }

        private static Process GetCommandProcess(string filename, string arguments)
        {
            return new Process
            {
                StartInfo =
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
        }
    }
}