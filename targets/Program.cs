using System.IO;
using static Bullseye.Targets;
using static SimpleExec.Command;

internal class Program
{
    public static void Main(string[] args)
    {
        Target("default", DependsOn("test"));

        Target("build",
            Directory.EnumerateFiles("src", "*.sln", SearchOption.AllDirectories),
            solution => Run("dotnet", $"build \"{solution}\" --configuration Release"));

        Target("test", DependsOn("build"),
            Directory.EnumerateFiles("src", "*Tests.csproj", SearchOption.AllDirectories),
            proj => Run("dotnet", $"test \"{proj}\" --configuration Release --no-build"));
        
        RunTargets(args);
    }
}
