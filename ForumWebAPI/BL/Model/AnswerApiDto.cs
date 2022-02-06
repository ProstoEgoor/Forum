using System;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {

    public class AnswerEditApiDto {
        public string Text { get; set; }

        public void Update(AnswerDbDTO answer) {
            answer.AnswerText = Text ?? answer.AnswerText;

            answer.ChangeDate = DateTime.Now;
        }
    }

    public class AnswerCreateApiDto : AnswerEditApiDto {
        public long QuestionId { get; set; }

        public AnswerDbDTO Create(string authorId) {
            return new AnswerDbDTO() {
                QuestionId = QuestionId,
                CreateDate = DateTime.Now,
                AuthorId = authorId,
                AnswerText = Text
            };
        }
    }

    public class AnswerApiDto : AnswerCreateApiDto {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Author { get; set; }
        public int Rating => VotePositive - VoteNegative;
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }

        public bool? MyVote { get; set; }

        public AnswerApiDto() { }

        public AnswerApiDto (AnswerDbDTO answer) {
            Id = answer.AnswerId;
            QuestionId = answer.QuestionId;
            CreateDate = answer.CreateDate;
            ChangeDate = answer.ChangeDate;
            Author = answer.Author?.UserName;
            Text = answer.AnswerText;
            VotePositive = answer.VotePositive;
            VoteNegative = answer.VoteNegative;
        }
    }
}
