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

        string _curLine;
        bool ReadLine()
        {
            do
            {
                _curLine = _input.ReadLine();
            } while (_curLine != null && _curLine.StartsWith("#"));
            return _curLine != null;
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

                int[] nums = _curLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                System.Diagnostics.Debug.Assert(nums.Length >= 4);
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
                        msg.AppendFormat("Вроде ответов должно быть {0}, а я нашел {1}", answerCount, question.Answers.Count);
                        answers = question.Answers;
                    }
                    else
                    {
                        msg.AppendFormat("Вроде правильных ответов должно быть {0}, а я нашел {1}", correctAnswerCount, question.CorrectAnswerCount);
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
                var msg = new StringBuilder();
                msg.AppendFormat("Чё-то не так с вопросом '{1}'", question.Index, question.Text).AppendLine();
                msg.Append("Мне не нравится, что ").Append(ex.Message);
                throw new ApplicationException(msg.ToString());
            }
        }
        public List<Question> ReadQuestions()
        {
            var questions = new List<Question>();

            while (ReadLine())
            {
                var question = ReadQuestion();
                if (question == null)
                    throw new ApplicationException("Cannot process: " + _curLine);

                questions.Add(question);
                continue;
            }

            return questions;
        }

        public List<Question> Parse(TextReader input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            _input = input;
            try
            {
                return ReadQuestions();
            }
            finally
            {
                _input = null;
            }
        }
    }
}
