using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LaDOSE.DesktopApp.Utils;
using LaDOSE.DTO;

namespace LaDOSE.DesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for BookingUserControl.xaml
    /// </summary>
    public partial class BookingUserControl : UserControl
    {

        public readonly String[] EventManagerField = new[] {"HR3", "HR2", "COMMENT", "BOOKING_COMMENT"};

        public String Current
        {
            get { return (String)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(String), typeof(BookingUserControl), new PropertyMetadata("",PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BookingUserControl uc = d as BookingUserControl;
            if (uc != null)
            {
                uc.Parse((string) e.NewValue);
            }
        }


        public ObservableCollection<Reservation> Reservation { get; set; }


        private void Parse(string value)
        {
            Reservation.Clear();
            var games = WpEventDeserialize.Parse(value);
            if (games != null) games.OrderBy(e => e.Name).ToList().ForEach(res => Reservation.Add(res));
        }

    

        public BookingUserControl()
        {
            InitializeComponent();
            Reservation = new ObservableCollection<Reservation>();
            this.DataContext = this;
            
        }


    }


}
