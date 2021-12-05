using System;
using System.Collections.Generic;

#nullable disable

namespace ForumDBContext.ModelDB​
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public string AnswerText { get; set; }
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }
        public int? Rating { get; set; }

        public virtual Question Question { get; set; }
    }
}
