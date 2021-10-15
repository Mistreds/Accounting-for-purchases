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
       private  void OnComboboxTextChanged(TextBox tb)
        {
           
            if (tb.SelectionStart != 0)
            {
                SpravCombo.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
            }
            if (tb.SelectionStart == 0 && SpravCombo.SelectedItem == null)
            {
                SpravCombo.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, сбросить фокус выпадающего списка
            }

            SpravCombo.IsDropDownOpen = true;
            if (SpravCombo.SelectedItem == null)
            {
                // Если элемент не выбран менять фильтр
                try
                {
                    Console.WriteLine(tb.Text);
                    SpravCombo.ItemsSource = new ObservableCollection<Model.Sprav>(MainViewModel.SelMain().Direct1.Where(p => p.Product.ToLower().Contains(tb.Text.ToLower())).ToList());
                    try
                    {

                    }
                    catch { Console.WriteLine("312312"); }

                }
                catch (Exception es)
                {
                    Console.WriteLine(es.Message);

                }


            }
        }

        private void SpravCombo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ss = (ListBox)SpravCombo.Template.FindName("ListBoxSprav", SpravCombo);
            var text = (TextBox)SpravCombo.Template.FindName("InpTextSprav", SpravCombo);
           
            switch (e.Key)
            {

                case Key.Up:
                    if (!ss.IsFocused)
                    {
                        ss.Focus();
                        var listBoxItem =
   (ListBoxItem)ss
     .ItemContainerGenerator
       .ContainerFromItem(ss.SelectedItem);

                        if (listBoxItem != null)
                            listBoxItem.Focus();
                    }
                    break;
                case Key.Down:
                    if (!ss.IsFocused)
                    {
                        ss.Focus();
                        var listBoxItem =
   (ListBoxItem)ss
     .ItemContainerGenerator
       .ContainerFromItem(ss.SelectedItem);
                        if (listBoxItem != null)
                            listBoxItem.Focus();
                    }
                    break;
                case Key.Enter:
                    
                        Console.WriteLine("dsda");
                        Model.Sprav a = (Model.Sprav)ss.SelectedItem;
                        MainViewModel.SelMain().AddInOrder(a);
                        text.Text = null;
                        ss.SelectedItem = null;
                        SpravCombo.SelectedItem = null;
                    break;
            }
            }
        private void SpravCombo_KeyDown(object sender, KeyEventArgs e)
        {
            var ss = (ListBox)SpravCombo.Template.FindName("ListBoxSprav", SpravCombo);
            var text = (TextBox)SpravCombo.Template.FindName("InpTextSprav", SpravCombo);
            if (e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Enter)
            {
                if (!text.IsFocused)
                {
                    Console.WriteLine(SpravCombo.IsFocused);
                    text.Focus();
                    OnComboboxTextChanged(text);
                }
                else
                {
                    OnComboboxTextChanged(text);
                }
            }
           
        }
        private void ListBoxSprav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox s = sender as ListBox;
            Model.Sprav a = (Model.Sprav)s.SelectedItem;
            var text = (TextBox)SpravCombo.Template.FindName("InpTextSprav", SpravCombo);
            try
            {


                text.Text = a.Product;
                text.CaretIndex = text.Text.Length;
                SpravCombo.Text = a.Product;
            }
            catch { }

        }

        private void SpravCombo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox s = sender as ListBox;
            Model.Sprav a = (Model.Sprav)s.SelectedItem;
            SpravCombo.IsDropDownOpen = false;
            MainViewModel.SelMain().AddInOrder(a); 
            var text = (TextBox)SpravCombo.Template.FindName("InpTextSprav", SpravCombo);
            text.Text = null;
            s.SelectedItem = null;
            SpravCombo.SelectedItem = null;
            

        }

        private void SpravCombo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("dsada");
            var text = (TextBox)SpravCombo.Template.FindName("InpTextSprav", SpravCombo);
            if(string.IsNullOrEmpty(text.Text))
            {
                SpravCombo.ItemsSource = new ObservableCollection<Model.Sprav>(MainViewModel.SelMain().Direct1.ToList());
                SpravCombo.IsDropDownOpen = true;
                
            }
        }
    }
        
          
           
    }
