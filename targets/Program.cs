using System.IO;
using System.Threading.Tasks;
using static Bullseye.Targets;
using static SimpleExec.Command;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var sdk = new DotnetSdkManager();
        var dotnet = await sdk.GetDotnetCliPath();

        Target("default", DependsOn("test"));

        Target("build",
            Directory.EnumerateFiles("src", "*.sln", SearchOption.AllDirectories),
            solution => Run(dotnet, $"build \"{solution}\" --configuration Release"));

        Target("test", DependsOn("build"),
            Directory.EnumerateFiles("src", "*Tests.csproj", SearchOption.AllDirectories),
            proj => Run(dotnet, $"test \"{proj}\" --configuration Release --no-build"));
        
        await RunTargetsAndExitAsync(args);
    }
}
