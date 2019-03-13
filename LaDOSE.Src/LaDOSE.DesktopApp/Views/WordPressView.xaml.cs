using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LaDOSE.DTO;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListViewItem = System.Windows.Controls.ListViewItem;
using UserControl = System.Windows.Controls.UserControl;

namespace LaDOSE.DesktopApp.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class WordPressView : UserControl
    {
        public WordPressView()
        {
       
            InitializeComponent();
        }


        private void Copy(object sender, KeyEventArgs e)
        {
            if (sender != BookingList) return;

            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            CopySelectedValuesToClipboard();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (WPBookingDTO item in BookingList.SelectedItems)
                builder.AppendLine(item.WpUser.Name);

            Clipboard.SetText(builder.ToString());
        }
    }
}
