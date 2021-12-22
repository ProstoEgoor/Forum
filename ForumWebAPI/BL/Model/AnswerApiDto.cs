using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class AnswerApiDto {
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public string Text { get; set; }
        public int Rating => (int)VotePositive - (int)VoteNegative;
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }

        public AnswerApiDto() { }
        
        public AnswerApiDto(AnswerDbDTO answer) {
            Id = answer.AnswerId;
            Author = answer.AuthorName;
            CreateDate = answer.CreateDate;
            Text = answer.AnswerText;
            VotePositive = answer.VotePositive;
            VoteNegative = answer.VoteNegative;
        }
    }
}
