using System.ComponentModel;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.ViewModels;

public class InfoViewModel:  ReactiveObject, IRoutableViewModel,INotifyPropertyChanged
{
   
    public InfoViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
    
    public string? UrlPathSegment => "Info";
    public IScreen HostScreen { get; }
}