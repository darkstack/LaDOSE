using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive
    {
     
        
        protected override void OnInitialize()
        {
            this.DisplayName = "LaDOSE";
            this.AppIcon = BitmapFrame.Create(Application.GetResourceStream(new Uri("/LaDOSE.DesktopApp;component/Resources/64x64.png",
                UriKind.RelativeOrAbsolute)).Stream);
            var wordPressViewModel = new WordPressViewModel(IoC.Get<RestService>());
            ActivateItem(wordPressViewModel);
            base.OnInitialize();
            
            
        }

        public BitmapFrame AppIcon { get; set; }

        public void LoadEvent()
        {
         ActivateItem(new WordPressViewModel(IoC.Get<RestService>()));
        }
        public void LoadGames()
        {
            ActivateItem(new GameViewModel(IoC.Get<RestService>()));
        }

        public void OpenWeb()
        {
            ActivateItem(new WebNavigationViewModel("www.google.com"));
        }
    }
}