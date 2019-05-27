using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using LaDOSE.REST;
using LaDOSE.REST.Event;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private string _user;

        public string User
        {
            get => _user;
            set
            {
                _user = value; 
                NotifyOfPropertyChange(User);
            }
        }

        protected override void OnInitialize()
        {
            this.DisplayName = "LaDOSE";
            this.User = "Test";
            this.AppIcon = BitmapFrame.Create(Application.GetResourceStream(new Uri("/LaDOSE.DesktopApp;component/Resources/64x64.png",
                UriKind.RelativeOrAbsolute)).Stream);

       
            var appSettings = ConfigurationManager.AppSettings;
            string url = (string)appSettings["ApiUri"];
            string user = (string)appSettings["ApiUser"];
            string password = (string)appSettings["ApiPassword"];
            Uri uri = new Uri(url);
            var restService = IoC.Get<RestService>();
            restService.UpdatedJwtEvent += TokenUpdate;
            restService.Connect(uri, user, password);
            var wordPressViewModel = new WordPressViewModel(IoC.Get<RestService>());
            ActivateItem(wordPressViewModel);
            base.OnInitialize();
            
            
        }

        private void TokenUpdate(object sender, UpdatedJwtEventHandler e)
        {
            this.User = e.Message.FirstName;
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

        public void TournamentResult()
        {
            ActivateItem(new TournamentResultViewModel());
        }
    }
}