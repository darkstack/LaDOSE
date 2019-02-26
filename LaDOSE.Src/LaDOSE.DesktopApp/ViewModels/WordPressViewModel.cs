using System.Collections.Generic;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;
using LaDOSE.DTO;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class WordPressViewModel : Screen
    {
        private RestService RestService { get; set; }
        public WordPressViewModel(RestService restService)
        {
            this.RestService = restService;
        }

        public void LoadEvents()
        {
            this.Events = this.RestService.GetEvents();
            NotifyOfPropertyChange("Events");
        }

        public List<WPEvent> Events { get; set; }
    }
}