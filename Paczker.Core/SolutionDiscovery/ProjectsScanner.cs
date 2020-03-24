using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using LanguageExt;
using net.r_eg.MvsSln;
using Paczker.Domain.Model;

namespace Paczker.Core.SolutionDiscovery
{
    public static class ProjectsScanner
    {
        public static IEnumerable<Option<Project>> GetAllProjectsInSln(string slnPath)
        {
            using var sln = new Sln(slnPath, SlnItems.Projects);
            return sln.Result.ProjectItems.Select(x =>
            {
                var fullPath = Path.GetFullPath(x.fullPath.Replace('\\', '/'));
                return ProjectsFactory.MapCsProj(GetProjectNameFromPath(fullPath), fullPath);
            });
        }

        public static string GetProjectNameFromPath(string csprojPath)
        {
            return Path.GetFileNameWithoutExtension(csprojPath);
        }
    }
}