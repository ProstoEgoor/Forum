using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class TagInQuestionDbDTO {
        public int QuestionId { get; set; }
        public string TagName { get; set; }

        public QuestionDbDTO Question { get; set; }
    }
}
