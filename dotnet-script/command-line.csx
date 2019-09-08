public class ExecutionException : Exception
{
    public int ExitCode { get; set; }

    public ExecutionException(int exitCode, string message) : base(message)
    {
        ExitCode = exitCode;
    }
}

public static class CommandLine
{
    public static void CheckResult(string command)
    {
        var result = GetExecuteResult(command);

        if (result.ExitCode != 0)
        {
            throw new ExecutionException(result.ExitCode, result.StandardOutput);
        }
    }

    public static ExecuteResult GetExecuteResult(string command)
    {
        command = command.Replace("\"", "\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/C \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            }
        };

        proc.Start();
        proc.WaitForExit();

        return new ExecuteResult
        {
            ExitCode = proc.ExitCode,
            StandardError = proc.StandardError.ReadToEnd().Trim(),
            StandardOutput = proc.StandardOutput.ReadToEnd().Trim()
        };
    }
}

public class ExecuteResult
{
    public int ExitCode { get; set; }
    public string StandardOutput { get; set; }
    public string StandardError { get; set; }
}