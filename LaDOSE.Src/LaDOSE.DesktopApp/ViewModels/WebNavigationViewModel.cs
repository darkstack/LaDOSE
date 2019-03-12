using Caliburn.Micro;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class WebNavigationViewModel : Screen
    {
        public WebNavigationViewModel(string uri)
        {
            Uri = uri;
            this.DisplayName = Uri;
        }

        public string Uri { get; set; }
    }
}