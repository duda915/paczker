using System;
using System.Diagnostics;
using System.IO;
using LanguageExt;
using Paczker.Infrastructure;

namespace Paczker.Core
{
    public class Pusher
    {
        public static Unit PushToSource(Project project, string source, string buildProfile)
        {
            var pathToPackage = Path.Combine(Path.GetDirectoryName(project.Path), "bin", buildProfile,
                $"{project.Name.Replace(".csproj", string.Empty).Replace(".fsproj", string.Empty)}.{project.Version}.nupkg");
            
            Process cmd = new Process();
            cmd.StartInfo.FileName = "dotnet";
            cmd.StartInfo.Arguments = $"nuget push --source {source} {pathToPackage}";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            LoggerFactory.LogInfo(cmd.StandardOutput.ReadToEnd());
            
            return Unit.Default;
        }
        
        public static Unit Build(string path, string buildProfile)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "dotnet";
            cmd.StartInfo.Arguments = $"build -c {buildProfile} {path}";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            LoggerFactory.LogInfo(cmd.StandardOutput.ReadToEnd());
            
            return Unit.Default;
        }
    }
}