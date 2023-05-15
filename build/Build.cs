using Nuke.Common.Execution;
using Nuke.Useful.Builds;

[UnsetVisualStudioEnvironmentVariables]
class Build : AzureDevOpsLibraryBuild
{
    public Build() => Platform = "x64";

    public static int Main() => Execute<Build>(x => x.RunAllSteps);
}
