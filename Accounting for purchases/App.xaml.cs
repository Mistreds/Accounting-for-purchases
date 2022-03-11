using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Accounting_for_purchases
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool is_connect;
        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            Autorization autorization;
            Setting.InitSetting();
            
                autorization = new Autorization();
                autorization.Show();

           

        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            MessageBox.Show($"{e.Exception.Message} {e.Exception.StackTrace}", "Ошибка");
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
