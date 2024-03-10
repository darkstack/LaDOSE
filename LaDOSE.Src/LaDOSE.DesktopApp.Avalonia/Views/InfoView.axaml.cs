using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LaDOSE.DesktopApp.Avalonia.ViewModels;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.Views;

public partial class InfoView : UserControl, IViewFor<InfoViewModel>
{
    // private AvaloniaCefBrowser browser;
    public InfoView()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (InfoViewModel?)value;
    }

    public InfoViewModel? ViewModel { get; set; }
}