using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {

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