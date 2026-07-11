using System.IO;

namespace MonitorInputSwitcher;

public record Settings(string Desktop, string Laptop)
{
    public static Settings Load()
    {
        var raw = File.ReadAllText("settings");
        var split = raw.Split(',');
        var desktop = split[0];
        var laptop = split[1];
        return new Settings(desktop, laptop);
    }
}