using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ForumDbContext.Model.DTO {
    public class QuestionDbDTO {
        public long QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string AuthorId { get; set; }
        public string Topic { get; set; }
        public string QuestionText { get; set; }

        public ICollection<AnswerDbDTO> Answers { get; set; }
        public ICollection<TagInQuestionDbDTO> Tags { get; set; }
        public UserDbDTO Author { get; set; }
    }
}
