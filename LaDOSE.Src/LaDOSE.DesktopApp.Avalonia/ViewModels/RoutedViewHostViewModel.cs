using System.ComponentModel;
using System.Runtime.CompilerServices;
using LaDOSE.DesktopApp.Avalonia.Utils;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.ViewModels;

public class RoutedViewHostViewModel : ReactiveObject, IScreen, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _current;

    public RoutedViewHostViewModel()
    {
        Games = new GamesViewModel(this);
        Info = new InfoViewModel(this);
        Tournament = new TournamentResultViewModel(this);
        Router.Navigate.Execute(Tournament);
        Current = "Tournament";

    }

    public string Current
    {
        get => _current;
        set
        {
            _current = value;
            RaisePropertyChanged(nameof(Current));
        }
    }

    public RoutingState Router { get; } = new();
    public GamesViewModel Games { get; }
    public InfoViewModel Info { get; }
    
    public TournamentResultViewModel Tournament { get; }
    

    public void ShowGames()
    {
        Router.Navigate.Execute(Games);
        Current = "Games";
    }

    public void ShowInfo()
    {
        Router.Navigate.Execute(Info);
        Current = "Info";
    }
    public void ShowTournament()
    {
        Router.Navigate.Execute(Tournament);
        Current = "Tournament";
    }
}
