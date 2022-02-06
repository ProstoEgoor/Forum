using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class AnswerDbDTO {
        public long AnswerId { get; set; }
        public long QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string AuthorId { get; set; }
        public string AnswerText { get; set; }
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }
        public int? Rating { get; set; }

        public QuestionDbDTO Question { get; set; }
        public UserDbDTO Author { get; set; }
        public ICollection<VoteDbDTO> Votes { get; set; }
    }
}
