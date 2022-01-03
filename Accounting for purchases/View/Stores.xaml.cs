using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Accounting_for_purchases.View
{
    /// <summary>
    /// Логика взаимодействия для Stores.xaml
    /// </summary>
    public partial class Stores : UserControl
    {
        public Stores(ViewModel.AdminViewModel adminViewModel)
        {
            InitializeComponent();
            DataContext = adminViewModel;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
        
        }

        private void StoreTable_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {          
            MainViewModel.AdminViewModel.StoreTable= new ObservableCollection<Model.Store>((IEnumerable<Model.Store>)StoreTable.ItemsSource);
        }
    }
}
