using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumDbContext.Model.DTO;

namespace ForumConsole.DB.Mapping {
    static class AnswerMapper {
        public static Answer Map(AnswerDbDTO answer) {
            if (answer == null) {
                return null;
            }

            return new Answer(answer.Rating ?? 0, answer.VotePositive + answer.VoteNegative) {
                Id = answer.AnswerId,
                Author = answer.AuthorName,
                Date = answer.CreateDate,
                Text = answer.AnswerText,
            };
        }

        public static AnswerDbDTO Map(Answer answer) {
            if (answer == null) {
                return null;
            }

            return new AnswerDbDTO() {
                AnswerId = answer.Id ?? 0,
                CreateDate = answer.Date,
                AuthorName = answer.Author,
                AnswerText = answer.Text,
                VotePositive = (uint)((answer.NumberOfVotes + answer.Rating) / 2),
                VoteNegative = (uint)((answer.NumberOfVotes - answer.Rating) / 2)
            };
        }
    }
}
