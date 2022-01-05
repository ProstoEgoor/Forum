using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class VoteDbDTO {
        public long AnswerId { get; set; }
        public string AuthorId { get; set; }
        public bool Vote { get; set; }

        public AnswerDbDTO Answer { get; set; }
        public UserDbDTO Author { get; set; }
    }
}
