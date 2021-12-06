using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    class AnswerDbDTO {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public string AnswerText { get; set; }
        public uint VotePositive { get; set; }
        public uint VoteNegative { get; set; }
        public int? Rating { get; set; }

        public QuestionDbDTO Question { get; set; }
    }
}
