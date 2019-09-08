#load "command-line.csx"

public class DotnetCommands
{
    public static void BuildCode() => CommandLine.CheckResult("dotnet build -c Release");
    public static void TestCode() => CommandLine.CheckResult("dotnet test -c Release");
}