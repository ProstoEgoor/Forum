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

            return new Answer(answer.VotePositive, answer.VoteNegative) {
                Id = answer.AnswerId,
                Author = answer.AuthorId,
                CreateDate = answer.CreateDate,
                ChangeDate = answer.ChangeDate,
                Text = answer.AnswerText,
            };
        }

        public static AnswerDbDTO Map(Answer answer) {
            if (answer == null) {
                return null;
            }

            return new AnswerDbDTO() {
                AnswerId = answer.Id ?? 0,
                CreateDate = answer.CreateDate,
                ChangeDate = answer.ChangeDate,
                AuthorId = answer.Author,
                AnswerText = answer.Text,
                VotePositive = answer.VotePositive,
                VoteNegative = answer.VoteNegative
            };
        }
    }
}
