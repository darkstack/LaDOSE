using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LaDOSE.DesktopApp.Avalonia.ViewModels;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.Views;

public partial class GamesView : UserControl,IViewFor<GamesViewModel>
{
    public GamesView()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (GamesViewModel?)value;
    }

    public GamesViewModel? ViewModel { get; set; }
}