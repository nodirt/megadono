using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILog
    {
        public void Transition(UIElement content)
        {
            placeholder.Children.Clear();
            placeholder.Children.Add(content);
        }
        void ShowError(string text)
        {
            Transition(new ErrorView(text));
        }

        public MainWindow()
        {
            InitializeComponent();

            App.Log = this;
            WriteLine("Программа запущена");
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            WriteLine("Конец");
        }

        public void WriteLine(string message)
        {
            string text = string.Format("[{0}] {1}{2}", DateTime.Now.ToString(), message, Environment.NewLine);
            log.AppendText(text);

            try
            {
                File.AppendAllText("megadono.log", text, Encoding.UTF8);
            }
            catch (IOException)
            {
            }

            log.ScrollToEnd();

        }
        public void WriteException(Exception ex, bool stack = true)
        {
            string message;
            if (stack)
            {
                message = ex.ToString();
            }
            else
            {
                message = string.Empty;
                for (; ex != null; ex = ex.InnerException)
                {
                    if (message.Length > 0)
                        message += Environment.NewLine;
                    message += ex.Message;
                }
            }

            WriteLine(message);
        }
    }

    interface ILog
    {
        void WriteLine(string message);
        void WriteException(Exception ex, bool stack = true);
    }
}
