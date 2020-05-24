using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.NUnit;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using static Nuke.Common.Tools.NUnit.NUnitTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(
        x => x.Clean,
        x => x.UnitTests,
        x => x.Pack
        );

    [Parameter("Nuget package version.")]
    readonly string PackageVersion;

    [Parameter("build in Debug.")]
    readonly bool Debug;

    Configuration GetConfiguration() => Debug ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath ArtifactsDirectory => RootDirectory / "bin";

    Target Clean => _ => _
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Compile => _ => _
        .Executes(() =>
        {
            CompileForPlatform(MSBuildTargetPlatform.x86);
            CompileForPlatform(MSBuildTargetPlatform.x64);
        });

    private void CompileForPlatform(MSBuildTargetPlatform platform)
    {
        var gribApiNative = RootDirectory.Parent / "GribApi.Native";
        var gribApiNativeBin = gribApiNative / "bin" / GetConfiguration() / "Grib.Api" / "lib" / "win" / platform.ToString();

        var targetFolder = ArtifactsDirectory / platform.ToString() / GetConfiguration() / "Grib.Api";
        var targetLibFolder = targetFolder / "lib" / "win" / platform.ToString();

        CopyFile(
            source: gribApiNativeBin / "Grib.Api.Native.dll",
            target: targetLibFolder / "Grib.Api.Native.dll",
            policy: FileExistsPolicy.OverwriteIfNewer);

        CopyFile(
            source: gribApiNativeBin / "Grib.Api.Native.Pdb",
            target: targetLibFolder / "Grib.Api.Native.Pdb",
            policy: FileExistsPolicy.OverwriteIfNewer);

        var gribApiXP = gribApiNative / "GribApi.XP" / "grib_api";

        CopyDirectoryRecursively(
            source: gribApiXP / "definitions",
            target: targetFolder / "definitions",
            directoryPolicy: DirectoryExistsPolicy.Merge,
            filePolicy: FileExistsPolicy.OverwriteIfNewer);

        MSBuild(s => s
            .SetRestore(true)
            .SetVerbosity(MSBuildVerbosity.Minimal)
            .SetProjectFile(Solution.GetProject("Grib.Api.Tests"))
            .SetTargetPlatform(platform)
            .SetConfiguration(GetConfiguration()));
    }

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            NUnit3(s => s
                .EnableNoHeader()
                .SetLabels(NUnitLabelType.Off)
                .EnableNoResults()
                .SetConfiguration(GetConfiguration())
                .SetInputFiles(ArtifactsDirectory / "x64" / GetConfiguration() / "Grib.Api.Tests.dll"));

            NUnit3(s => s
                .EnableX86()
                .EnableNoHeader()
                .SetLabels(NUnitLabelType.Off)
                .EnableNoResults()
                .SetConfiguration(GetConfiguration())
                .SetInputFiles(ArtifactsDirectory / "x86" / GetConfiguration() / "Grib.Api.Tests.dll"));
        });

    AbsolutePath NugetDirectory => RootDirectory / "nuget.package";

    Target Pack => _ => _
        .DependsOn(UnitTests)
        .Requires(() => PackageVersion)
        .Executes(() =>
        {
            NuGetPack(s => s
                .DisableBuild()
                .SetBasePath(NugetDirectory)
                .SetTargetPath(NugetDirectory / "Grib.Api.nuspec")
                .SetVersion(PackageVersion)
                .SetOutputDirectory(NugetDirectory)
            );
        });
}
