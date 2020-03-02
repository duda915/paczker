using System;
using System.IO;
using Paczker.Core.Commands;
using Paczker.Infrastructure;
using Serilog;
using Serilog.Events;

namespace Paczker
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
            
            var command = args[0];

            switch (command)
            {
                case "deps":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    var projectName = args[2];
                    ListDependentProjectsCommand.ListDependentProjects(path, projectName);
                    break;
                }
                case "list":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    ListAllProjectsCommand.ListAllProjects(path);
                    break;
                }
                case "inc":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    var projectName = args[2];
                    var versionPart = args[3];
                    IncrementProjectsCommand.IncrementProjects(path, projectName, versionPart);
                    break;
                }
                case "setpre":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    var projectName = args[2];
                    IncrementProjectsCommand.SetPre(path, projectName);
                    break;
                }
                case "rmpre":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    var projectName = args[2];
                    IncrementProjectsCommand.RemovePre(path, projectName);
                    break;
                }
                case "push":
                {
                    var path = $"{Path.GetFullPath(args[1])}/";
                    LoggerFactory.LogInfo($"Running in path {path}");
                    var projectName = args[2];
                    var source = args[3];
                    var buildProfile = args[4];
                    IncrementProjectsCommand.Push(path, projectName, source, buildProfile);
                    break;
                }
            }
        }
    }
}