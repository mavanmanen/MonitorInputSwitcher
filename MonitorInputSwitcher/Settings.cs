using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MonitorInputSwitcher;

public partial record Settings(string DesktopHostname, string DesktopMonitor, string DesktopValue, string LaptopHostname, string LaptopMonitor, string LaptopValue)
{
    public static Settings Load()
    {
        var raw = File.ReadAllText("settings");
        var matches = SettingsRegex().Matches(raw);

        if (matches.Count == 0)
        {
            throw new Exception("Settings aren't following the correct format.");
        }

        var desktopHostname = matches[0].Groups["hostname"].Value.Trim();
        var desktopMonitor = matches[0].Groups["monitor"].Value.Trim();
        var desktopValue = matches[0].Groups["value"].Value.Trim();

        var laptopHostname = matches[1].Groups["hostname"].Value.Trim();
        var laptopMonitor = matches[1].Groups["monitor"].Value.Trim();
        var laptopValue = matches[1].Groups["value"].Value.Trim();

        return new Settings(desktopHostname, desktopMonitor, desktopValue, laptopHostname, laptopMonitor, laptopValue);
    }

    [GeneratedRegex(@"\[\w+\]\r?\nHostname=(?<hostname>.+)\r?\nMonitor=(?<monitor>\d+)\r?\nValue=(?<value>\d+)", RegexOptions.Compiled)]
    private static partial Regex SettingsRegex();
}