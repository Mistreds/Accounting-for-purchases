using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : UserControl
    {
        public Employees(ViewModel.AdminViewModel adminViewModel)
        {
            InitializeComponent();
            DataContext = adminViewModel;
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            MainViewModel.AdminViewModel.Employees = new System.Collections.ObjectModel.ObservableCollection<Model.Employee>((IEnumerable<Model.Employee>)EmpTable.ItemsSource);
        }
    }
}
