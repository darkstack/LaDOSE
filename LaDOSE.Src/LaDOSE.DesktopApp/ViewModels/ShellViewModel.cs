using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using LaDOSE.REST;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive
    {
     
        
        protected override void OnInitialize()
        {
            this.DisplayName = "LaDOSE";
            this.AppIcon = BitmapFrame.Create(Application.GetResourceStream(new Uri("/LaDOSE.DesktopApp;component/Resources/64x64.png",
                UriKind.RelativeOrAbsolute)).Stream);

            var appSettings = ConfigurationManager.AppSettings;
            string url = (string)appSettings["ApiUri"];
            string user = (string)appSettings["ApiUser"];
            string password = (string)appSettings["ApiPassword"];
            Uri uri = new Uri(url);
            var restService = IoC.Get<RestService>();

            restService.Connect(uri, user, password);

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