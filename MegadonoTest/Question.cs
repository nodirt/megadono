using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadonoTest
{
    public class Question
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public List<Answer> Answers { get; private set; }
        public int PointPerAnswer { get; set; }

        public int CorrectAnswerCount
        {
            get { return Answers.Count(a => a.IsCorrect); }
        }
        public int MaxPoints
        {
            get { return PointPerAnswer * CorrectAnswerCount; }
        }

        public Question()
        {
            Answers = new List<Answer>();
            PointPerAnswer = 1;
        }
    }

    public class Answer 
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
