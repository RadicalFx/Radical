#r "packages/Bullseye.1.0.0-rc.5/lib/netstandard2.0/Bullseye.dll"
#r "packages/SimpleExec.2.2.0/lib/netstandard2.0/SimpleExec.dll"

using System;
using static Bullseye.Targets;
using static SimpleExec.Command;

var vswhere = "packages/vswhere.2.1.4/tools/vswhere.exe";
var nuget = ".nuget/v4.3.0/NuGet.exe";
string msBuild = null;

var solutions = Directory.EnumerateFiles(".", "*.sln", SearchOption.AllDirectories);

Add("default", DependsOn("build"));

Add(
    "restore",
    () =>
    {
        foreach (var solution in solutions)
        {
            Run(nuget, $"restore {solution}");
        }
    });

Add(
    "find-msbuild",
    () => msBuild = $"{Read(vswhere, "-latest -requires Microsoft.Component.MSBuild -property installationPath").Trim()}/MSBuild/15.0/Bin/MSBuild.exe");

Add(
    "build",
    DependsOn("find-msbuild", "restore"),
    () =>
    {
        foreach (var solution in solutions)
        {
            Run(msBuild, $"{solution} /p:Configuration=Debug /nologo /m /v:m /nr:false");
        }
    });

Run(Args);
