using System;
using System.Linq;
using LanguageExt;
using Serilog;

namespace Paczker.Infrastructure
{
    public static class LoggerFactory
    {
        public static Unit LogInfo(string message)
        {
            Log.Information(message);
            return Unit.Default;
        }
    }
}