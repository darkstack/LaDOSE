using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;

namespace LaDOSE.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
      
            MessageBox.Show("WAITING IN DEBUG MODE");     
#endif
            base.OnStartup(e);


            var settings = new CefSettings();
            settings.SetOffScreenRenderingBestPerformanceArgs();
            Cef.Initialize(settings);
        }


    }
}
