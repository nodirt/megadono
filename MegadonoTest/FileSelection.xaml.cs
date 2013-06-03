using System;
using System.IO;
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

namespace MegadonoTest
{
    /// <summary>
    /// Interaction logic for FileSelection.xaml
    /// </summary>
    public partial class FileSelection : UserControl
    {
        public FileSelection()
        {
            InitializeComponent();

            fileNames.ItemsSource = Directory.GetFiles(QuestionStorage.QuestionDirectory).Select(Path.GetFileName);
            if (fileNames.Items.Count > 0)
            {
                fileNames.SelectedIndex = 0;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string fileName = (string) fileNames.SelectedItem;
            if (fileName == null)
                return;

            fileName = Path.Combine(QuestionStorage.QuestionDirectory, fileName);

            try
            {
                var questions = new QuestionStorage();
                questions.Load(fileName);
                App.Log.WriteLine(string.Format("Загружено {0} вопросов", questions.Questions.Count));
                App.MainWindow.Transition(new TestView(questions));
            }
            catch (Exception ex)
            {
                App.Log.WriteException(ex);
                return;
            }
        }
    }
}
