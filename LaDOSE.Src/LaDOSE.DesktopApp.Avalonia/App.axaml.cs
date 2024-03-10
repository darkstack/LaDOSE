using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LaDOSE.DesktopApp.Avalonia.ViewModels;
using LaDOSE.DesktopApp.Avalonia.Views;
using ReactiveUI;
using Splat;

namespace LaDOSE.DesktopApp.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Locator.CurrentMutable.Register(() => new GamesView(), typeof(IViewFor<GamesViewModel>));
        Locator.CurrentMutable.Register(() => new InfoView(), typeof(IViewFor<InfoViewModel>));
        Locator.CurrentMutable.Register(() => new TournamentResultView(), typeof(IViewFor<TournamentResultViewModel>));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}