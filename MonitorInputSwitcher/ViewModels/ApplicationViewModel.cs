using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonitorInputSwitcher.Services;

namespace MonitorInputSwitcher.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    private readonly Settings _settings;
    private readonly MonitorService _monitorService;
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
        DesktopValue = _settings.Desktop;
        LaptopValue = _settings.Laptop;
        
        _monitorService = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => new WindowsMonitorService(),
            PlatformID.Unix => new LinuxMonitorService(),
            _ => throw new PlatformNotSupportedException()
        };
        
        var current = _monitorService.GetCurrentInput();
        if (current != null)
        {
            SetHeaders(current);
        }
    }

    [RelayCommand]
    private void SwitchInput(string value)
    {
        _monitorService.ChangeInput(value);
        SetHeaders(value);
    }

    private void SetHeaders(string value)
    {
        if (value == _settings.Desktop)
        {
            DesktopHeader = $"{Desktop} {Checkmark}";
            LaptopHeader = Laptop;
        }
        else if(value == _settings.Laptop)
        {
            DesktopHeader = Desktop;
            LaptopHeader = $"{Laptop} {Checkmark}";
        }
    }

    [RelayCommand]
    private void Quit() => Environment.Exit(0);
}