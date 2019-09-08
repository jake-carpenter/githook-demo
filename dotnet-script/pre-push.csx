#!/usr/bin/env dotnet dotnet-script
#load "git-commands.csx"
#load "dotnet-commands.csx"

var exitCode = 0;
bool stashedChanges = GitCommands.CheckForUnstagedChanges();

try
{
    if (stashedChanges)
    {
        CommandLine.Write("--> Found unstaged changes. Stashing...", ConsoleColor.Yellow);
        GitCommands.StashChanges();
    }

    CommandLine.Write("--> Building solution...", ConsoleColor.Yellow);
    DotnetCommands.BuildCode();

    CommandLine.Write("--> Running tests...", ConsoleColor.Yellow);
    DotnetCommands.TestCode();

    CommandLine.Write("--> Success!", ConsoleColor.Green);
}
catch (System.Exception ex)
{
    GitCommands.UnstashChanges();

    exitCode = ex is ExecutionException execEx ? execEx.ExitCode : -1;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
}
finally
{
    CommandLine.Write("--> Unstashing changes...", ConsoleColor.Yellow);

    if (stashedChanges)
    {
        GitCommands.UnstashChanges();
    }

    Environment.Exit(exitCode);
}



