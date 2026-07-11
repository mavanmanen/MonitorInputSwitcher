using System.Diagnostics;

namespace MonitorInputSwitcher.Services;

public abstract class MonitorService
{
    public abstract string? GetCurrentInput();
    
    public abstract bool ChangeInput(string value);
    
    protected static (string output, string error) RunCommand(string command, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
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