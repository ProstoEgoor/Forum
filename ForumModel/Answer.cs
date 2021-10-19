using System;

namespace ForumModel {
    public class Answer {
        public string Author { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Text { get; set; }
        public int Rating { get; private set; } = 0;
        public uint NumberOfVotes { get; private set; } = 0;

        public Answer() { }
        public Answer(int rating, uint numberOfVotes) {
            Rating = rating;
            NumberOfVotes = (uint) Math.Min(Math.Abs(rating), numberOfVotes);
        }

        public int Vote(int rating) {
            Rating += rating;
            NumberOfVotes += (uint) Math.Abs(rating);
            return Rating;
        }

        public override string ToString() {
            string answer = "Автор: \t\t" + Author + Environment.NewLine;
            answer += "Дата: \t\t" + Date + Environment.NewLine;
            answer += "Рейтинг: \t" + Rating + Environment.NewLine;
            answer += "< " + Text + " >";
            return answer;
        }

    }
}