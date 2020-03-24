using System.Collections.Generic;
using LanguageExt.Common;

namespace Paczker.Infrastructure.Command
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Result<IEnumerable<string>> Handle(T message);
    }
}