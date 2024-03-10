
using System.Data;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using LaDOSE.DesktopApp.Avalonia.ViewModels;
using ReactiveUI;

namespace LaDOSE.DesktopApp.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class TournamentResultView : UserControl, IViewFor<TournamentResultViewModel>
    {
        public TournamentResultView()
        {
            InitializeComponent();
        }


        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TournamentResultViewModel)value;
        }

        public TournamentResultViewModel? ViewModel { get; set; }

        private void DataGrid_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "ItemsSource")
            {
                Trace.WriteLine("Changed Binding");
                 
                var grid = (sender as DataGrid);
                grid.Columns.Clear();
                var data = ViewModel.GridDataTable;

                foreach (DataColumn? view in data.Columns)
                {
                    
                    grid.Columns.Add(new DataGridTextColumn()
                    {
                        Header = view.ColumnName,
                        CanUserSort = true,
                        Binding = new Binding($"Row.ItemArray[{view.Ordinal}]") 
                    });
                }
            }
            return;
        }
    }
}
