using System;
using System.Collections.Generic;
using LanguageExt.Common;
using Paczker.Facade.Commands.DecrementProjects;
using Paczker.Facade.Commands.IncrementProjects;
using Paczker.Facade.Commands.ListAllProjects;
using Paczker.Facade.Commands.ListDependentProjects;
using Paczker.Facade.Commands.PushProjects;
using Paczker.Facade.Commands.RemovePreReleaseVersion;
using Paczker.Facade.Commands.SetPreReleaseVersion;
using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands
{
    public static class CommandHandlerInvoker
    {
        public static Result<IEnumerable<string>> GetHandlerResult<T>(T command) where T : ICommand
        {
            return command switch
            {
                DecrementProjectsCommand c => new DecrementProjectsCommandHandler().Handle(c),
                IncrementProjectsCommand c => new IncrementProjectsCommandHandler().Handle(c),
                ListAllProjectsCommand c => new ListAllProjectsCommandHandler().Handle(c),
                ListDependentProjectsCommand c => new ListDependentProjectsCommandHandler().Handle(c),
                PushProjectsCommand c => new PushProjectsCommandHandler().Handle(c),
                RemovePreReleaseVersionCommand c => new RemovePreReleaseVersionCommandHandler().Handle(c),
                SetPreReleaseVersionCommand c => new SetPreReleaseVersionCommandHandler().Handle(c),
                _ => throw new Exception("unknown command")
            };
        }
    }
}