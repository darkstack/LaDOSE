using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace LaDOSE.DesktopApp.Avalonia.ViewModels;

public class MainWindowViewModel : Window
{
    public RoutedViewHostViewModel RoutedViewViewHost { get; } = new();

    public void CloseApp()
    {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            ((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!).Shutdown();
    }
}