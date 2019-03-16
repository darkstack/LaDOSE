using System;
using System.IO;

namespace LaDOSE.DiscordBot.Service
{
    public class TodoService
    {
        private const string db = "todo.txt";

        public TodoService()
        {
        }

        public bool Add(string text)
        {
            if (!string.IsNullOrEmpty(text)) { 
                using (var textWriter =File.AppendText(db))
                {
                    textWriter.WriteLine(text);
                }
                return true;
            }

            return false;

        }
        public bool Delete(int id)
        {
            string returnText = "";
            var text = File.ReadAllText(db);
            var i = 0;
            foreach (var line in text.Split('\n'))
            {
                ++i;
                if (i != id)
                {
                    returnText += $"{line}\n";
                }
                
            }

            File.WriteAllText(db,returnText);
            return true;

        }

        public string List()
        {
            string returnText = "";
            var text = File.ReadAllText(db);
            var i = 0;
            foreach (var line in text.Split())
            {
                if(!string.IsNullOrEmpty(line))
                returnText += $"{++i}. {line}";
            }

            return returnText;
        }
    }
}