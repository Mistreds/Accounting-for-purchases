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
using System.Windows.Shapes;

namespace Accounting_for_purchases
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
            
            Loaded += Autorization_Loaded;
        }

        private void Autorization_Loaded(object sender, RoutedEventArgs e)
        {
            GetDatabaseSetting();
        }

        public Autorization(bool is_not_connect)
        {
            InitializeComponent();
            GetDatabaseSetting();
        }
        private void GetDatabaseSetting()
        {
            BaseDB.Text = Setting.GetDatabase();
            BaseServer.Text = Setting.GetServer();
            BaseLogin.Text = Setting.GetUser();
            BasePass.Text = Setting.GetPassword();
        }
        private void SaveBase_Click(object sender, RoutedEventArgs e)
        {
            Setting.SetDatabaseSetting(BaseServer.Text, BaseLogin.Text, BasePass.Text, BaseDB.Text);
            using (var db = new ConnectDB()) { }
            if (App.is_connect)
            {
                this.EditBase.Visibility = Visibility.Collapsed;
                this.MainAut.Visibility = Visibility.Visible;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.EditBase.Visibility =Visibility.Collapsed;
            this.MainAut.Visibility = Visibility.Visible;
        }

        private void OpenSetting_Click(object sender, RoutedEventArgs e)
        {
            this.EditBase.Visibility = Visibility.Visible;
            this.MainAut.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ConnectDB())
            {
                var Emp = db.Employee.Where(p => p.Login == Login.Text && p.Password == Password.Password).FirstOrDefault();
                if (Emp != null)
                {
                    MainWindow main = new MainWindow(Emp);
                    main.Show();
                    Close();
                }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
