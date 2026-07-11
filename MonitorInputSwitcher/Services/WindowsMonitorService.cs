using System.Text.RegularExpressions;

namespace MonitorInputSwitcher.Services;

public partial class WindowsMonitorService : MonitorService
{
    public override string? GetCurrentInput(string monitor)
    {
        var (output, error) = RunCommand("winddcutil.exe", $"getvcp {monitor} 0x60");
        if (error.Length > 1)
        {
            return null;
        }

        var match = CurrentInputRegex().Match(output);
        if (!match.Success)
        {
            return null;
        }
        
        var value = match.Groups["Value"].Value.Trim();
        return value;
    }

    public override bool ChangeInput(string monitor, string value)
    {
        var (_, error) = RunCommand("winddcutil.exe", $"setvcp {monitor} 0x60 {value}");
        return error.Length == 0;
    }

    [GeneratedRegex(@"VCP 0x60 (?<Value>\d{2})")]
    private static partial Regex CurrentInputRegex();
}