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

namespace MegadonoTest
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        public ResultsView(TestResults results)
        {
            if (results == null)
                throw new ArgumentNullException("results");
            InitializeComponent();
            DataContext = results;
        }

        private void start(object sender, RoutedEventArgs e)
        {
            App.MainWindow.Transition(new TestView());
        }

        private void close(object sender, RoutedEventArgs e)
        {
            App.MainWindow.Close();
        }
    }

    public class TestResults 
    {
        public int QuestionCount {get;set;}
        public int CorrectCount { get; set; }
        public int MaxPoints { get; set; }
        public int GotPoints { get; set; }
    }
}
