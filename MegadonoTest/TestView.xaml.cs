using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        QuestionStorage _storage;
        List<TestQuestion> _questions;
        int _currentIndex = 0;
        int _maxIndex = 0;

        TestQuestion CurrentQuestion
        {
            get { return _questions[_currentIndex]; }
        }

        public TestView(QuestionStorage storage)
        {
            InitializeComponent();

            _storage = storage;
            _questions = storage.Questions.Shuffle()
                .Select(q => new TestQuestion(q))
                .ToList();

            GoTo(0);

            App.Log.WriteLine("Начался тест " + storage.Name);
        }

        void GoTo(int questionIndex)
        {
            _currentIndex = questionIndex;
            _maxIndex = Math.Max(_maxIndex, _currentIndex);
            var question = CurrentQuestion;
            this.question.Text = question.Question.Text;
            points.Text = question.Question.PointPerAnswer.ToString();
            var correctCount = question.Question.CorrectAnswerCount;
            correctAnswerCount.Text = correctCount.ToString();

            answers.Children.Clear();
            
            for(int i = 0; i < question.Answers.Count; i++)
            {
                ToggleButton btn = correctCount == 1 ? new RadioButton() as ToggleButton : new CheckBox();
                btn.IsChecked = question.Answers[i].IsChecked;
                btn.Content = new TextBox
                {
                    Text = question.Answers[i].Answer.Text,
                    IsReadOnly = true,
                    BorderThickness = new Thickness(),
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                answers.Children.Add(btn);
            }
            UpdateView();
        }

        void SaveState()
        {
            var ans = CurrentQuestion.Answers;
            for (int i = 0; i < answers.Children.Count; i++)
            {
                var btn = (ToggleButton)answers.Children[i];
                ans[i].IsChecked = btn.IsChecked == true;
            }
        }

        void UpdateView()
        {
            backBtn.IsEnabled = _currentIndex > 0;
            forwardBtn.IsEnabled = _currentIndex + 1 < _questions.Count;
            questionIndex.Text = (_currentIndex + 1).ToString();
            questionCount.Text = (_maxIndex + 1).ToString();
        }
        private void back(object sender, RoutedEventArgs e)
        {
            SaveState();
            GoTo(_currentIndex - 1);
        }

        private void forward(object sender, RoutedEventArgs e)
        {
            SaveState();
            GoTo(_currentIndex + 1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void finish(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно?", "Всё что-ли?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            SaveState();

            var answeredQuestions = _questions.Take(_maxIndex + 1).ToList();
            var results = new TestResults
            {
                Storage = _storage,
                QuestionCount = _maxIndex + 1,
                MaxPoints = answeredQuestions.Sum(a => a.MaxPoints),
                GotPoints = answeredQuestions.Sum(a => a.GotPoints)
            };
            App.MainWindow.Transition(new ResultsView(results));
        }
    }

    class TestQuestion
    {
        public Question Question { get; private set; }
        public List<TestAnswer> Answers { get; private set; }

        public int MaxPoints
        {
            get { return Question.MaxPoints; }
        }
        public int GotPoints
        {
            get { return Question.PointPerAnswer * Answers.Count(a => a.IsChecked && a.Answer.IsCorrect); }
        }

        public TestQuestion(Question question)
        {
            if (question == null)
                throw new ArgumentNullException("question");
            this.Question = question;
            this.Answers = question.Answers.Shuffle().Select(a => new TestAnswer(a)).ToList();
        }
    }
    class TestAnswer
    {
        public Answer Answer { get; private set; }
        public bool IsChecked { get; set; }

        public TestAnswer(Answer answer)
        {
            this.Answer = answer;
        }
    }
}
