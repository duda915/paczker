using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Paczker.Core.Nuget;
using Paczker.Core.SolutionDiscovery;
using Paczker.Infrastructure.Command;
using Paczker.Infrastructure.Exceptions;

namespace Paczker.Facade.Commands.PushProjects
{
    public class PushProjectsCommandHandler : ICommandHandler<PushProjectsCommand>
    {
        public Result<IEnumerable<string>> Handle(PushProjectsCommand message)
        {
            var projects = ProjectsScanner.GetAllProjectsInSln(message.SlnPath)
                .Choose(x => x).ToList();

            return Prelude.Optional(DependencyTree.FindReferences(projects, message.ProjectNames)
                    .SelectMany(x =>
                    {
                        var buildLog = Pusher.Build(x, message.BuildProfile);
                        var pushLog = Pusher.PushToSource(x, message.Source, message.BuildProfile);

                        return new[] {buildLog, pushLog};
                    }).DefaultIfEmpty())
                .Match(x => new Result<IEnumerable<string>>(x),
                    new Result<IEnumerable<string>>(new NothingFoundException("no projects were found")));
        }
    }
}