using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace LaDOSE.DesktopApp.Behaviors
{
    public static class HtmlBehavior // : Behavior<ChromiumWebBrowser>
    {
        #region DependencyProperties

        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlBehavior),
                new PropertyMetadata("",OnHtmlChange));

        private static void OnHtmlChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChromiumWebBrowser)
            {
                var cwb = ((ChromiumWebBrowser) d);
                string html = e.NewValue as string;
                if (!string.IsNullOrEmpty(html))
                    
                    Application.Current.Dispatcher.Invoke(()=>
                    {
                        try
                        {
                            
                            if (cwb.IsBrowserInitialized)
                            {
                                cwb.LoadHtml(html, false);
                            }
                            else
                            {
                                cwb.IsBrowserInitializedChanged += CwbOnIsBrowserInitializedChanged;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                      
                            
                    });
                

            }
        }

        private static void CwbOnIsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var cwb = ((ChromiumWebBrowser)sender);
            var html = GetHtml(cwb);    
            cwb.LoadHtml(html, false);
            cwb.IsBrowserInitializedChanged -= CwbOnIsBrowserInitializedChanged;
        }

        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }
        #endregion
    }
}