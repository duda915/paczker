using System;

namespace Paczker.Core.Tests
{
    public static class TestPathHelper
    {
        public static string GetTestProjectPath(string projectName)
            => $@"{Environment.CurrentDirectory}/TestSolution/{projectName}/{projectName}.csproj";

        public static string GetTestSolutionPath()
            => $@"{Environment.CurrentDirectory}/TestSolution";
    }
}