using Avalonia;
using Avalonia.Markup.Xaml;
using MonitorInputSwitcher.ViewModels;

namespace MonitorInputSwitcher;

public partial class App : Application
{
    public App()
    {
        DataContext = new ApplicationViewModel();
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}