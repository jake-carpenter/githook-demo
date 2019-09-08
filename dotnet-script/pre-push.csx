#!/usr/bin/env dotnet dotnet-script
#load "git-commands.csx"
#load "dotnet-commands.csx"
#load "logger.csx"

public void DisplayActionResult(string msg, Action action)
{
    Logger.LogStartProcess(msg);
    action.Invoke();
    Logger.LogFinishProcess(msg);
}

Console.OutputEncoding = System.Text.Encoding.UTF8;
var exitCode = 0;
bool unstagedChanges = GitCommands.CheckForUnstagedChanges();
var msg = "";

try
{
    if (unstagedChanges)
    {
        msg = "Found unstaged changes. Stashing...";
        DisplayActionResult(msg, () => GitCommands.StashChanges());
    }

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
    if (unstagedChanges)
    {
        DisplayActionResult("Unstashing changes...", () => GitCommands.UnstashChanges());
    }

    Environment.Exit(exitCode);
}




