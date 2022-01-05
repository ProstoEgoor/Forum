using System;
using System.Collections.Generic;
using System.Linq;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {

    public class QuestionEditApiDto {
        public string Topic { get; set; }
        public string Text { get; set; }
        public IEnumerable<TagApiDto> Tags { get; set; }

        public void Update(QuestionDbDTO question) {
            question.Topic = Topic ?? question.Topic;
            question.QuestionText = Text ?? question.QuestionText;
            if (Tags != null)
                question.Tags = Tags.Select(tag => tag.Create(question.QuestionId)).ToList();

            question.ChangeDate = DateTime.Now;
        }
    }

    public class QuestionCreateApiDto : QuestionEditApiDto {

        public QuestionDbDTO Create(string authorId) {
            return new QuestionDbDTO() {
                CreateDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                AuthorId = authorId,
                Topic = Topic,
                QuestionText = Text,
                Tags = Tags.Select(tag => tag.Create()).ToList()
            };
        }
    }

    public class QuestionApiDto : QuestionCreateApiDto {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Author { get; set; }
        public int AnswerCount { get; set; }

        public QuestionApiDto() { }

        public QuestionApiDto(QuestionDbDTO question) {
            Id = question.QuestionId;
            CreateDate = question.CreateDate;
            ChangeDate = question.ChangeDate;
            Author = question.Author?.UserName;
            Topic = question.Topic;
            Text = question.QuestionText;
            AnswerCount = question.Answers?.Count ?? 0;
            Tags = question.Tags.Select(tag => new TagApiDto(tag));
        }
    }

    public class QuestionDetailApiDto : QuestionApiDto {
        public IEnumerable<AnswerApiDto> Answers { get; set; }

        public QuestionDetailApiDto(QuestionDbDTO question) : base(question) {
            Answers = question.Answers.Select(answer => new AnswerApiDto(answer));
        }

    }
}
