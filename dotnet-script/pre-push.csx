#!/usr/bin/env dotnet dotnet-script
#load "git-commands.csx"
#load "dotnet-commands.csx"
#load "logger.csx"

var protectedBranches = new[] { "master" };
var exitCode = 0;
bool unstagedChanges = false;
var msg = "";

try
{
    ShouldRun();
    unstagedChanges = StashUnstagedChanges();

    msg = "Building solution...";
    DisplayActionResult(msg, () => DotnetCommands.BuildCode());

    msg = "Running tests...";
    DisplayActionResult(msg, () => DotnetCommands.TestCode());
}
catch (System.Exception ex)
{
    Logger.LogFailProcess(msg, ex.Message);
    exitCode = ex is ExecutionException execEx ? execEx.ExitCode : -1;
}
finally
{
    CheckForChangesToUnstash(unstagedChanges);
    Environment.Exit(exitCode);
}

private void ShouldRun()
{
    if (!GitCommands.IsProtectedBranch(protectedBranches))
    {
        Environment.Exit(exitCode);
    }
}

private bool StashUnstagedChanges()
{
    if (GitCommands.CheckForUnstagedChanges())
    {
        msg = "Found unstaged changes. Stashing...";
        DisplayActionResult(msg, () => GitCommands.StashChanges());

        return true;
    }

    return false;
}

private void CheckForChangesToUnstash(bool stashed)
{
    if (stashed)
    {
        DisplayActionResult("Unstashing changes...", () => GitCommands.UnstashChanges());
    }
}

public void DisplayActionResult(string msg, Action action)
{
    Logger.LogStartProcess(msg);
    action.Invoke();
    Logger.LogFinishProcess(msg);
}