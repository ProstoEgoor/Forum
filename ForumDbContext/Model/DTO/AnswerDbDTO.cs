using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class AnswerDbDTO {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public string AnswerText { get; set; }
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }
        public int? Rating { get; set; }

        public QuestionDbDTO Question { get; set; }
    }
}
