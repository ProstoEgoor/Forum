using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class QuestionApiDto {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Author { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public int AnswerCount { get; set; }
        public IEnumerable<TagApiDto> Tags { get; set; }

        public QuestionApiDto() { }

        public QuestionApiDto(QuestionDbDTO question) {
            Id = question.QuestionId;
            CreateDate = question.CreateDate;
            Author = question.AuthorName;
            Topic = question.Topic;
            Text = question.QuestionText;
            AnswerCount = question.Answers.Count;
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
