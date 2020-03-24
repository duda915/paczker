using Paczker.Infrastructure.Command;

namespace Paczker.Facade.Commands.ListAllProjects
{
    public class ListAllProjectsCommand : ICommand
    {
        public readonly string SlnPath;

        public ListAllProjectsCommand(string slnPath)
        {
            SlnPath = slnPath;
        }
    }
}