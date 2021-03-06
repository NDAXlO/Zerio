#l "scripts/utilities.cake"
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.FileHelpers"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var paths = new {
    solution = MakeAbsolute(File("./../src/Abc.Zerio.sln")).FullPath,
    version = MakeAbsolute(File("./../version.yml")).FullPath,
    assemblyInfo = MakeAbsolute(File("./../src/SharedVersionInfo.cs")).FullPath,
    output = new {
        build = MakeAbsolute(Directory("./../output/build")).FullPath,
        nuget = MakeAbsolute(Directory("./../output/nuget")).FullPath,
    },
    nuspec = MakeAbsolute(File("./Abc.Zerio.nuspec")).FullPath,
};

ReadContext(paths.version);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("UpdateBuildVersionNumber").Does(() => UpdateAppVeyorBuildVersionNumber());
Task("Clean").Does(() =>
{
    CleanDirectory(paths.output.build);
    CleanDirectory(paths.output.nuget);
});
Task("Restore-NuGet-Packages").Does(() => NuGetRestore(paths.solution));
Task("Create-AssemblyInfo").Does(() => {
    CreateAssemblyInfo(paths.assemblyInfo, new AssemblyInfoSettings {
        Version = VersionContext.AssemblyVersion,
        FileVersion = VersionContext.AssemblyVersion,
        InformationalVersion = VersionContext.NugetVersion + " Commit: " + VersionContext.Git.Sha
    });
});
Task("MSBuild").Does(() => MSBuild(paths.solution, settings => settings.SetConfiguration("Release")
                                                                        .SetPlatformTarget(PlatformTarget.MSIL)
                                                                        .WithProperty("OutDir", paths.output.build)));
Task("Clean-AssemblyInfo").Does(() => FileWriteText(paths.assemblyInfo, string.Empty));
Task("Run-Unit-Tests").Does(() => NUnit3(paths.output.build + "/*.Tests.dll", new NUnit3Settings { NoResults = true }));
Task("Nuget-Pack").Does(() => 
{
    NuGetPack(paths.nuspec, new NuGetPackSettings {
        Version = VersionContext.NugetVersion,
        BasePath = paths.output.build,
        OutputDirectory = paths.output.nuget
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("UpdateBuildVersionNumber")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Create-AssemblyInfo")
    .IsDependentOn("MSBuild")
    .IsDependentOn("Clean-AssemblyInfo");

Task("Test")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests");

Task("Nuget")
    .IsDependentOn("Test")
    .IsDependentOn("Nuget-Pack")
    .Does(() => {
        Information("   Nuget package is now ready at location: {0}.", paths.output.nuget);
        Warning("   Please remember to create and push a tag based on the currently built version.");
        Information("   You can do so by copying/pasting the following commands:");
        Information("       git tag v{0}", VersionContext.NugetVersion);
        Information("       git push origin --tags");
    });

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
