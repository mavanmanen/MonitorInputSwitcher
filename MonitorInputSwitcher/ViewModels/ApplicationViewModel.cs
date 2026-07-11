using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonitorInputSwitcher.Services;

namespace MonitorInputSwitcher.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    private readonly Settings _settings;
    private readonly MonitorService _monitorService;
    private readonly string _hostname;
    private const string Desktop = "Desktop";
    private const string Laptop = "Laptop";
    private const string Checkmark = "✔";

    [ObservableProperty]
    public partial string DesktopHeader { get; set; } = Desktop;
    
    [ObservableProperty]
    public partial string DesktopValue { get; set; }

    [ObservableProperty]
    public partial string LaptopHeader { get; set; } = Laptop;

    [ObservableProperty]
    public partial string LaptopValue { get; set; }
    
    public ApplicationViewModel()
    {
        _settings = Settings.Load();
        DesktopValue = _settings.DesktopValue;
        LaptopValue = _settings.LaptopValue;
        
        _monitorService = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => new WindowsMonitorService(),
            PlatformID.Unix => new LinuxMonitorService(),
            _ => throw new PlatformNotSupportedException()
        };

        var hostname = _monitorService.GetHostname();
        _hostname = hostname ?? throw new Exception("Could not find hostname");
        
        var current = _monitorService.GetCurrentInput(GetMonitorNumber());
        if (current != null)
        {
            SetHeaders(current);
        }
    }

    private string GetMonitorNumber()
    {
        if (_hostname == _settings.DesktopHostname)
        {
            return _settings.DesktopMonitor;
        }

        if (_hostname == _settings.LaptopHostname)
        {
            return _settings.LaptopMonitor;
        }

        throw new ArgumentOutOfRangeException();
    }

    [RelayCommand]
    private void SwitchInput(string value)
    {
        _monitorService.ChangeInput(GetMonitorNumber(), value);
        SetHeaders(value);
    }

    private void SetHeaders(string value)
    {
        if (value == _settings.DesktopValue)
        {
            DesktopHeader = $"{Desktop} {Checkmark}";
            LaptopHeader = Laptop;
        }
        else if(value == _settings.LaptopValue)
        {
            DesktopHeader = Desktop;
            LaptopHeader = $"{Laptop} {Checkmark}";
        }
    }

    [RelayCommand]
    private void Quit() => Environment.Exit(0);
}