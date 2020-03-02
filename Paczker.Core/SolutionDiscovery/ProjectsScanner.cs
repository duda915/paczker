using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt.Common;
using Paczker.Infrastructure.Exception;

namespace Paczker.Core.SolutionDiscovery
{
    public static class ProjectsScanner
    {
        public static Result<IEnumerable<string>> GetAllProjectsInDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                  return new Result<IEnumerable<string>>(new NotExistsException($"Directory {path} not exists"));
            }

            var directories = Directory.GetDirectories(path);

            var projects = directories
                .SelectMany(Directory.GetFiles)
                .Where(x => x.EndsWith(".csproj") || x.EndsWith(".fsproj"));
            
            return new Result<IEnumerable<string>>(projects);
        }
    }
}