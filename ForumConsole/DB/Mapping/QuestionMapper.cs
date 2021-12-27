using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ForumModel;
using ForumDbContext.Model.DTO;

namespace ForumConsole.DB.Mapping {
    static class QuestionMapper {
        public static Question Map(QuestionDbDTO question) {
            if (question == null) {
                return null;
            }

            return new Question(question.Tags.Select(tag => tag.TagName).ToList(),
                question.Answers.Select(answer => AnswerMapper.Map(answer)).ToList()) {
                Id = question.QuestionId,
                Author = question.AuthorId,
                CreateDate = question.CreateDate,
                ChangeDate = question.ChangeDate,
                Topic = question.Topic,
                Text = question.QuestionText
            };
        }

        public static QuestionDbDTO Map(Question question) {
            if (question == null) {
                return null;
            }

            return new QuestionDbDTO() {
                QuestionId = question.Id ?? 0,
                CreateDate = question.CreateDate,
                ChangeDate = question.ChangeDate,
                AuthorId = question.Author,
                Topic = question.Topic,
                QuestionText = question.Text,
                Answers = question.Answers.Select(answer => AnswerMapper.Map(answer)).ToList(),
                Tags = question.Tags.Select(tag => new TagInQuestionDbDTO() { TagName = tag }).ToList()
            };
        }
    }
}
