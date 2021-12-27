using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class AnswerApiDto {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public int Rating => VotePositive - VoteNegative;
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }

        public AnswerApiDto() { }
        
        public AnswerApiDto(AnswerDbDTO answer) {
            Id = answer.AnswerId;
            QuestionId = answer.QuestionId;
            CreateDate = answer.CreateDate;
            ChangeDate = answer.ChangeDate;
            AuthorId = answer.AuthorId;
            Text = answer.AnswerText;
            VotePositive = answer.VotePositive;
            VoteNegative = answer.VoteNegative;
        }

        public AnswerDbDTO Create() {
            return new AnswerDbDTO() {
                QuestionId = QuestionId,
                CreateDate = CreateDate,
                ChangeDate = ChangeDate,
                AuthorId = AuthorId,
                AnswerText = Text,
                VotePositive = VotePositive,
                VoteNegative = VoteNegative
            };
        }

        public void Update(AnswerDbDTO answer) {
            if (Text != null) {
                answer.AnswerText = Text;
            }
        }
    }
}
