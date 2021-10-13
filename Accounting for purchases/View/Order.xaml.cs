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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : UserControl
    {
        public Order()
        {
            InitializeComponent();
        }
        private string _filtered;
        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource;
            if (tb.SelectionStart != 0)
            {
                CB.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
            }
            if (tb.SelectionStart == 0 && CB.SelectedItem == null)
            {
                CB.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
            }

            CB.IsDropDownOpen = true;
            if (CB.SelectedItem == null)
            {
                // Если элемент не выбран менять фильтр
                _filtered = CB.Text;
                CB.ItemsSource = new ObservableCollection<Model.Sprav>(MainViewModel.SelMain().Direct1.Where(p => p.Product.ToLower().Contains(CB.Text)).ToList());
                

            }
        }
        public bool OnFilterTriggered(object item)
        {

            Model.Sprav ss = item as Model.Sprav;


            return ss.Product.ToLower().Contains(_filtered.ToLower());

        }

        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CB.SelectedItem = null;
        }
    }
}
