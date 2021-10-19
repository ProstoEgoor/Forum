using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class Question {
        public string Author { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        HashSet<string> TagSet { get; } = new HashSet<string>();
        public string Topic { get; set; }
        public string Text { get; set; }
        List<Answer> ListAnswer { get; } = new List<Answer>();

        IEnumerable<Answer> Answers => ListAnswer;
        IEnumerable<string> Tags => TagSet;

        public Question() { }

        public Question(IEnumerable<string> tags) {
            foreach (var tag in tags) {
                TagSet.Add(tag);
            }
        }

        public void AddAnswer(params Answer[] answers) {
            ListAnswer.AddRange(answers);
        }

        public override string ToString() {
            string answer = "Автор: \t" + Author + Environment.NewLine;
            answer += "Дата: \t" + Date + Environment.NewLine;
            answer += "Теги: \t" + String.Join(", ", Tags) + Environment.NewLine;
            answer += "Тема: \t" + Topic + Environment.NewLine;
            answer += "< " + Text + " >" + Environment.NewLine;
            answer += "Ответы: " + Environment.NewLine + Environment.NewLine;
            answer += String.Join(Environment.NewLine + Environment.NewLine, Answers);
            return answer;
        }

    }
}
