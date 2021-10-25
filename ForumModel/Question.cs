using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class Question {
        public string Author { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        protected HashSet<string> TagSet { get; } = new HashSet<string>();
        public string Topic { get; set; }
        public string Text { get; set; }
        protected List<Answer> ListAnswer { get; } = new List<Answer>();

        public IReadOnlyList<Answer> Answers => ListAnswer;
        public IEnumerable<string> Tags => TagSet;

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
            StringBuilder sb = new StringBuilder();
            sb.Append($"Тема:\t{Topic}\n\r");
            sb.Append($"Теги:\t{string.Join(", ", Tags)}\n\r");
            sb.Append($"Автор:\t{Author}\n\r");
            sb.Append($"Дата:\t{Date}\n\r");
            sb.Append($"{Answers.Count} Ответов\n\r");
            sb.Append(Text);
            return sb.ToString();
        }

    }
}
