using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadonoTest
{
    public class QuestionStorage
    {
        const string QuestionsFolderName = "questions";
        static string _dirPath;
        static QuestionStorage()
        {
            _dirPath = Path.Combine(Path.GetDirectoryName(typeof(QuestionStorage).Assembly.Location), QuestionsFolderName);
        }

        List<Question> _questions = new List<Question>();
        public List<Question> Questions
        {
            get { return _questions; }
        }

        static Encoding _windows1251 = Encoding.GetEncoding(1251);

        public void Load()
        {
            _questions.Clear();
            if (!Directory.Exists(_dirPath))
                throw new ApplicationException("Вы меня простите, конечно, но где папка с вопросами?? Вот эта:" + Environment.NewLine + _dirPath);

            var files = Directory.GetFiles(_dirPath, "*.txt");
            if (files.Length == 0)
                throw new ApplicationException("А где, собственно, файлы вопросов? Хотя бы один файл .txt был бы в папке " + Environment.NewLine + _dirPath);

            var parser = new QuestionParser();
            var errors = new List<ParseError>();
            foreach (var filename in files)
            {
                try
                {
                    using (var reader = new StreamReader(filename, _windows1251))
                    {
                        _questions.AddRange(parser.Parse(reader));
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new ParseError { Filename = filename, Exception = ex });
                }
            }

            if (_questions.Count == 0)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Я чё-то не понял!");
                foreach (var err in errors)
                {
                    msg.AppendFormat("Файл {0}:", Path.GetFileName(err.Filename)).AppendLine();
                    msg.AppendLine(err.Exception.Message);
                    msg.AppendLine();
                }
                msg.Append("Как-то так...");
                throw new ApplicationException(msg.ToString());
            }
        }

        class ParseError
        {
            public string Filename { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
