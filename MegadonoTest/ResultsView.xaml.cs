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
        TestResults _results;

        public ResultsView(TestResults results)
        {
            if (results == null)
                throw new ArgumentNullException("results");
            _results = results;
            InitializeComponent();
            DataContext = results;

            App.Log.WriteLine(string.Format("Кончился тест {0}", results.Storage.Name));
            App.Log.WriteLine(string.Format("Баллов {0}/{1}. Вопросов {2}/{3}", results.GotPoints, results.MaxPoints, results.CorrectCount, results.QuestionCount));
        }

        private void start(object sender, RoutedEventArgs e)
        {
            App.MainWindow.Transition(new TestView(_results.Storage));
        }

        private void close(object sender, RoutedEventArgs e)
        {
            App.MainWindow.Close();
        }
    }

    public class TestResults
    {
        public QuestionStorage Storage { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectCount { get; set; }
        public int MaxPoints { get; set; }
        public int GotPoints { get; set; }
    }
}
