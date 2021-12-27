using System;

namespace ForumModel {
    public class Answer {
        public long? Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ChangeDate { get; set; }
        public string Text { get; set; }
        public int Rating => VotePositive - VoteNegative;
        public int VotePositive { get; private set; } = 0;
        public int VoteNegative { get; private set; } = 0;

        public Answer() {
            if (ChangeDate == DateTime.MinValue) {
                ChangeDate = CreateDate;
            }
        }
        public Answer(int votePositive, int voteNegative) : this() {
            VotePositive = votePositive;
            VoteNegative = voteNegative;
        }

        public Answer(int rating) : this(rating >= 0 ? rating : 0, rating < 0 ? -rating : 0) { }

        public int Vote(bool votePositive) {
            if (votePositive) {
                VotePositive++;
            } else {
                VoteNegative++;
            }
            return Rating;
        }
    }
}