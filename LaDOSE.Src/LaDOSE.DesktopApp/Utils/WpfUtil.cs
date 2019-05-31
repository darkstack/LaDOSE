using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public static void Await(Action function,string message=null)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Task tsk = Task.Factory.StartNew(new Action(function));
    
            tsk.ContinueWith(t =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                        System.Windows.Input.Mouse.OverrideCursor = null);
                    MessageBox.Show(t.Exception.InnerException.Message);
                 
                },
                CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
            tsk.ContinueWith(task =>
            {
                if (!string.IsNullOrEmpty(message))
                    MessageBox.Show(message);
                Application.Current.Dispatcher.Invoke(() =>
                    System.Windows.Input.Mouse.OverrideCursor = null);
            });


        }

    }
}