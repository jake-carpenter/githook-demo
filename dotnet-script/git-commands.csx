#load "command-line.csx"

public class GitCommands
{
    public static bool StashChanges()
    {
        var result = CommandLine.GetExecuteResult("git stash --keep-index -q");

        return result.ExitCode != 0;
    }

    public static bool UnstashChanges()
    {
        var result = CommandLine.GetExecuteResult("git stash pop -q");

        return result.ExitCode != 0;
    }

    public static bool CheckForUnstagedChanges()
    {
        var result = CommandLine.GetExecuteResult("git diff --exit-code");

        return result.ExitCode != 0;
    }
}