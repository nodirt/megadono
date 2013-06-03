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
        public string Name { get; set; }

        const string QuestionsFolderName = "questions";
        public static readonly string QuestionDirectory;
        static QuestionStorage()
        {
            QuestionDirectory = Path.Combine(Path.GetDirectoryName(typeof(QuestionStorage).Assembly.Location), QuestionsFolderName);
            if (!Directory.Exists(QuestionDirectory))
                Directory.CreateDirectory(QuestionDirectory);
        }

        List<Question> _questions = new List<Question>();
        public List<Question> Questions
        {
            get { return _questions; }
        }

        public void Load(string filename)
        {
            var parser = new QuestionParser();
            try
            {
                using (var reader = new StreamReader(filename, Encoding.UTF8))
                {
                    _questions = parser.Parse(reader);
                }
            }
            catch (Exception ex)
            {
                var msg = new StringBuilder();
                msg.AppendLine("Я чё-то не понял!");
                msg.AppendFormat("Файл {0}:", Path.GetFileName(filename)).AppendLine();
                msg.AppendLine(ex.Message);
                msg.AppendLine();
                msg.Append("Как-то так...");
                throw new ApplicationException(msg.ToString());
            }

            Name = Path.GetFileNameWithoutExtension(filename);

            if (_questions.Count == 0)
            {
                throw new ApplicationException("Вопросов-то в файле нет!");
            }
        }
    }
}
