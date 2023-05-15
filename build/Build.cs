using Nuke.Common.Execution;
using Nuke.Useful.Builds;

[UnsetVisualStudioEnvironmentVariables]
class Build : AzureDevOpsLibraryBuild
{
    public static int Main() => Execute<Build>(x => x.RunAllSteps);
}
