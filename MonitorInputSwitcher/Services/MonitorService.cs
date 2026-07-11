using System;
using System.Diagnostics;

namespace MonitorInputSwitcher.Services;

public abstract class MonitorService
{
    public abstract string? GetCurrentInput(string monitor);
    
    public abstract bool ChangeInput(string monitor, string value);

    public string? GetHostname()
    {
        var (output, error) = RunCommand("hostname");
        if (error.Length > 0)
        {
            return null;
        }

        return output.Trim();
    }
    
    protected static (string output, string error) RunCommand(string command, string arguments = "")
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = Environment.CurrentDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };

        using var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return (output, error);
    }
}