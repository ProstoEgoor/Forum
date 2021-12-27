using System;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class TagInQuestionDbDTO {
        public long QuestionId { get; set; }
        public string TagName { get; set; }

        public QuestionDbDTO Question { get; set; }
    }
}
