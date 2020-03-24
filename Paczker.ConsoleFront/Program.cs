using System.Linq;
using CommandLine;
using Paczker.Facade.Commands.DecrementProjects;
using Paczker.Facade.Commands.IncrementProjects;
using Paczker.Facade.Commands.ListAllProjects;
using Paczker.Facade.Commands.ListDependentProjects;
using Paczker.Facade.Commands.PushProjects;
using Paczker.Facade.Commands.RemovePreReleaseVersion;
using Paczker.Facade.Commands.SetPreReleaseVersion;
using Paczker.Verbs;
using static Paczker.CommandToConsoleInterface;

namespace Paczker
{
    public class Program
    {
        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<DecOptions, DepsOptions, IncOptions, ListOptions, PushOptions, RmPreOptions, SetPreOptions>(args)
                .MapResult(
                    (DecOptions opts) =>
                        PrintAndReturn(new DecrementProjectsCommand(opts.SolutionPath, opts.ProjectNames.ToArray(),
                            opts.VersionPart)),
                    (DepsOptions opts) =>
                        PrintAndReturn(new ListDependentProjectsCommand(opts.SolutionPath, opts.ProjectNames.ToArray())),
                    (IncOptions opts) =>
                        PrintAndReturn(new IncrementProjectsCommand(opts.SolutionPath, opts.ProjectNames.ToArray(),
                            opts.VersionPart)),
                    (ListOptions opts) => PrintAndReturn(new ListAllProjectsCommand(opts.SolutionPath)),
                    (PushOptions opts) => PrintAndReturn(new PushProjectsCommand(opts.SolutionPath, opts.ProjectNames.ToArray(),
                        opts.Source, opts.BuildConfiguration)),
                    (RmPreOptions opts) =>
                        PrintAndReturn(new RemovePreReleaseVersionCommand(opts.SolutionPath, opts.ProjectNames.ToArray())),
                    (SetPreOptions opts) =>
                        PrintAndReturn(new SetPreReleaseVersionCommand(opts.SolutionPath, opts.ProjectNames.ToArray())),
                    errs => 1);
        }
    }
}