using System;
using Paczker.Facade.Commands;
using Paczker.Facade.Commands.ListAllProjects;
using Paczker.Infrastructure.Command;

namespace Paczker
{
    public static class CommandToConsoleInterface
    {
        public static int PrintAndReturn(ICommand command)
        {
            return CommandHandlerInvoker.GetHandlerResult(command)
                .Match(x =>
                {
                    x.Iter(Console.WriteLine);
                    return 0;
                }, x =>
                {
                    Console.WriteLine(x.Message);
                    return -1;
                });
        }
    }
}