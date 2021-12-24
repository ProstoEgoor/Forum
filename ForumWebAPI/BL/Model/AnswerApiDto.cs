using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class AnswerApiDto {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rating => VotePositive - VoteNegative;
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }

        public AnswerApiDto() { }
        
        public AnswerApiDto(AnswerDbDTO answer) {
            Id = answer.AnswerId;
            QuestionId = answer.QuestionId;
            Author = answer.AuthorName;
            CreateDate = answer.CreateDate;
            Text = answer.AnswerText;
            VotePositive = answer.VotePositive;
            VoteNegative = answer.VoteNegative;
        }

        public AnswerDbDTO Create() {
            return new AnswerDbDTO() {
                QuestionId = QuestionId,
                CreateDate = CreateDate,
                AuthorName = Author,
                AnswerText = Text,
                VotePositive = VotePositive,
                VoteNegative = VoteNegative
            };
        }

        public void Update(AnswerDbDTO answer) {
            if (Author != null)
                answer.AuthorName = Author;
            if (Text != null) {
                answer.AnswerText = Text;
            }
        }
    }
}
