using System;
using System.Text.RegularExpressions;

namespace MonitorInputSwitcher.Services;

public partial class LinuxMonitorService : MonitorService
{
    public override string? GetCurrentInput()
    {
        var (output, error) = RunCommand("ddcutil", "getvcp 60");
        if (error.Length > 1)
        {
            return null;
        }

        var match = CurrentInputRegex().Match(output);
        if (!match.Success)
        {
            return null;
        }

        var value = match.Groups["Value"].Value;
        var valueInt = Convert.ToInt32(value, 16).ToString();
        return valueInt;
    }

    public override bool ChangeInput(string value)
    {
        var (_, error) = RunCommand("ddcutil", $"setvcp 60 {value}");
        return error.Length == 0;
    }
    
    [GeneratedRegex(@".+\(sl=0x(?<Value>[0-9a-f]{2})\)")]
    private static partial Regex CurrentInputRegex();
}

public class WindowsMonitorService : MonitorService
{
    public override string? GetCurrentInput()
    {
        throw new NotImplementedException();
    }

    public override bool ChangeInput(string value)
    {
        throw new NotImplementedException();
    }
}