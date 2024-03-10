using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.Utils;

public abstract class BaseViewModel : ReactiveObject, IRoutableViewModel,INotifyPropertyChanged
{
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected BaseViewModel(IScreen hostScreen, string? urlPathSegment)
    {
        UrlPathSegment = urlPathSegment;
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
}