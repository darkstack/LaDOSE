using System;
using System.Collections.Generic;
using System.Windows;

namespace LaDOSE.DesktopApp.Utils
{
    public static class WpfUtil
    {
        public static void AddUI<T>(this ICollection<T> collection, T item, Action action = null)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
            if(action!=null)
                Application.Current.Dispatcher.BeginInvoke(action);
        }

    }
}