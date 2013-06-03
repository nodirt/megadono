using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadonoTest
{
    class QuestionParser
    {
        TextReader _input;

        List<Exception> _errors;
        string _curLine;
        bool ReadLine()
        {
            do
            {
                _curLine = _input.ReadLine();
            } while (_curLine != null && _curLine.StartsWith("#"));
            return _curLine != null;
        }
        void ReadUntilBlankLine()
        {
            while (ReadLine() && !string.IsNullOrWhiteSpace(_curLine))
            {
            }
        }
        Question ReadQuestion()
        {
            if (string.IsNullOrWhiteSpace(_curLine))
                return null;

            var question = new Question { Text = _curLine.Trim() };
            if (question.Text.EndsWith(":"))
            {
                question.Text = question.Text.Substring(0, question.Text.Length - 1);
            }

            try {

                if (!ReadLine())
                    return null;

                int[] nums;
                try
                {
                    nums = _curLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                }
                catch (FormatException)
                {
                    throw new ApplicationException("Числа в непонятном формате: " + _curLine);
                }
                if (nums.Length < 4)
                {
                    ReadUntilBlankLine();
                    throw new ApplicationException("Чисел должно быть минимум 5: " + _curLine);
                }

                question.Index = nums[0] + 1;
                int answerCount = nums[1];
                question.PointPerAnswer = nums[2];
                int correctAnswerCount = nums[3];
                int[] correctAnswers = nums.Skip(4).ToArray();

                while (ReadLine() && !string.IsNullOrWhiteSpace(_curLine))
                {
                    question.Answers.Add(new Answer
                    {
                        Text = _curLine,
                        IsCorrect = correctAnswers.Contains(question.Answers.Count + 1)
                    });
                }

                if (question.Answers.Count != answerCount || correctAnswerCount != question.CorrectAnswerCount)
                {
                    var msg = new StringBuilder();
                    IEnumerable<Answer> answers;
                    if (question.Answers.Count != answerCount)
                    {
                        msg.AppendFormat("Ответов должно быть {0}, а найдено {1}", answerCount, question.Answers.Count);
                        answers = question.Answers;
                    }
                    else
                    {
                        msg.AppendFormat("Правильных ответов должно быть {0}, а найдено {1}", correctAnswerCount, question.CorrectAnswerCount);
                        answers = question.Answers.Where(a => a.IsCorrect);
                    }
                    msg.AppendLine(":");
                    foreach (var ans in answers)
                    {
                        msg.AppendLine(ans.Text);
                    }

                    throw new ApplicationException(msg.ToString().Trim());
                }

                return question;
            }
            catch(Exception ex) {
                var msg = string.Format("Ошибка в вопросе '{1}'", question.Index, question.Text);
                throw new ApplicationException(msg, ex);
            }
        }
        public List<Question> ReadQuestions()
        {
            var questions = new List<Question>();

            while (ReadLine())
            {
                try
                {
                    var question = ReadQuestion();

                    if (question == null)
                        _errors.Add(new ApplicationException("Cannot process: " + _curLine));
                    else 
                        questions.Add(question);
                }
                catch (ApplicationException ex)
                {
                    _errors.Add(ex);
                }
            }

            return questions;
        }

        public QuestionParserResult Parse(TextReader input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            _input = input;
            _errors = new List<Exception>();
            try
            {
                return new QuestionParserResult { Questions = ReadQuestions(), Errors = _errors };
            }
            finally
            {
                _input = null;
            }
        }
    }

    class QuestionParserResult
    {
        public List<Question> Questions;
        public List<Exception> Errors;
    }
}
