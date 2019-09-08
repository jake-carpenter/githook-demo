#load "command-line.csx"

public class GitCommands
{
    public static void StashChanges()
    {
        CommandLine.GetExecuteResult("git stash -q --keep-index");
    }

    public static void UnstashChanges()
    {
        CommandLine.GetExecuteResult("git stash pop -q");
    }

    public static bool CheckForUnstagedChanges()
    {
        var result = CommandLine.GetExecuteResult("git diff --exit-code");

        return result.ExitCode != 0;
    }
}