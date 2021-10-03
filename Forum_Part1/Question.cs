using System;
using System.Collections.Generic;
using System.Text;

namespace Forum_Part1 {
    class Question {
        public Question(string author, DateTime date, string[] tags, string topic, string questionMsg) {
            Author = author;
            Date = date;
            Tags = new string[tags.Length];
            Array.Copy(tags, Tags, tags.Length);
            Topic = topic;
            QuestionMsg = questionMsg;
            Answers = new List<Answer>();
        }

        public string Author;
        public DateTime Date;
        public string[] Tags;
        public string Topic;
        public string QuestionMsg;
        public List<Answer> Answers;

        public void AddAnswer(Answer answer) {
            Answers.Add(answer);
        }

        public override string ToString() {
            string answer = "Автор: \t" + Author + Environment.NewLine;
            answer += "Дата: \t" + Date + Environment.NewLine;
            answer += "Теги: \t" + String.Join(", ", Tags) + Environment.NewLine;
            answer += "Тема: \t" + Topic + Environment.NewLine;
            answer += "< " + QuestionMsg + " >" + Environment.NewLine;
            answer += "Ответы: " + Environment.NewLine + Environment.NewLine;
            answer += String.Join(Environment.NewLine + Environment.NewLine, Answers.ToArray());
            return answer;
        }
    }
}
