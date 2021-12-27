using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class QuestionApiDto {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string AuthorId { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public int AnswerCount { get; set; }
        public IEnumerable<TagApiDto> Tags { get; set; }

        public QuestionApiDto() { }

        public QuestionApiDto(QuestionDbDTO question) {
            Id = question.QuestionId;
            CreateDate = question.CreateDate;
            ChangeDate = question.ChangeDate;
            AuthorId = question.AuthorId;
            Topic = question.Topic;
            Text = question.QuestionText;
            AnswerCount = question.Answers?.Count ?? 0;
            Tags = question.Tags.Select(tag => new TagApiDto(tag));
        }

        public QuestionDbDTO Create() {
            return new QuestionDbDTO() {
                CreateDate = CreateDate,
                ChangeDate = ChangeDate,
                AuthorId = AuthorId,
                Topic = Topic,
                QuestionText = Text,
                Tags = Tags.Select(tag => tag.Create()).ToList()
            };
        }

        public void Update(QuestionDbDTO question) {
            if (Topic != null)
                question.Topic = Topic;
            if (Text != null)
                question.QuestionText = Text;

            if (Tags != null)
                question.Tags = Tags.Select(tag => tag.Create(question.QuestionId)).ToList();

        }
    }

    public class QuestionDetailApiDto : QuestionApiDto {
        public IEnumerable<AnswerApiDto> Answers { get; set; }

        public QuestionDetailApiDto(QuestionDbDTO question) : base(question) {
            Answers = question.Answers.Select(answer => new AnswerApiDto(answer));
        }

    }
}
