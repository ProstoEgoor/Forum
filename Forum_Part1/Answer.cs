using System;

namespace Forum_Part1 {
    struct Answer {
        
        public Answer(string author, DateTime date, string answerMsg, int rating) {
            Author = author;
            Date = date;
            AnswerMsg = answerMsg;
            Rating = rating;
        }

        public string Author;
        public DateTime Date;
        public String AnswerMsg;
        public int Rating;

        public override string ToString() {
            string answer = "Автор: \t\t" + Author + Environment.NewLine;
            answer += "Дата: \t\t" + Date + Environment.NewLine;
            answer += "Рейтинг: \t" + Rating + Environment.NewLine;
            answer += "< " + AnswerMsg + " >";
            return answer;
        }
    }
}
