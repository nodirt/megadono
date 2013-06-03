using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MegadonoTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    partial class App : Application
    {
        public static new MainWindow MainWindow
        {
            get { return (MainWindow)Application.Current.MainWindow; }
        }
        public static new App Current
        {
            get { return (App)Application.Current; }
        }
        public static ILog Log { get; set; }

        public App()
        {
            StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
        }

        [STAThread]
        static void Main()
        {
            try
            {
                App app = new App();
                app.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ппц!");
            }
        }
    }
}
