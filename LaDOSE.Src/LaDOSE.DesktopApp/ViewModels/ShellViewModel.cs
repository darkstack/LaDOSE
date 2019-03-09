using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive
    {
        protected override void OnInitialize()
        {
            var wordPressViewModel = new WordPressViewModel(IoC.Get<RestService>());
            ActivateItem(wordPressViewModel);
            base.OnInitialize();
       
            
        }

        public void LoadEvent()
        {
         ActivateItem(new WordPressViewModel(IoC.Get<RestService>()));
        }
        public void LoadGames()
        {
            ActivateItem(new GameViewModel(IoC.Get<RestService>()));
        }

    }
}