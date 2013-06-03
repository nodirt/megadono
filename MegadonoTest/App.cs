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

        public QuestionStorage QuestionStorage { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            QuestionStorage = new QuestionStorage();
            try
            {
                QuestionStorage.Load();
            }
            catch (Exception ex)
            {
                MainWindow.Content = new ErrorView(ex.Message);
            }

            base.OnStartup(e);
        }

        [STAThread]
        static void Main()
        {
            try
            {
                App app = new App();
                app.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
                app.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ппц!");
            }
        }
    }
}
