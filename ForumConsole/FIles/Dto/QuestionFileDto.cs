using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ForumModel;
using System.Linq;

namespace ForumConsole.FIles {

    [XmlType(TypeName = "Question")]
    public class QuestionFileDto {
        //public long? Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public string[] Tags { get; set; }
        public AnswerFileDto[] Answers { get; set; }

        public static QuestionFileDto Map(Question question) {
            return new QuestionFileDto() {
                //Id = question.Id,
                Author = question.Author,
                CreateDate = question.CreateDate,
                ChangeDate = question.ChangeDate,
                Topic = question.Topic,
                Text = question.Text,
                Tags = question.Tags.ToArray(),
                Answers = question.Answers.Select(answer => AnswerFileDto.Map(answer)).ToArray(),
            };
        }

        public static Question Map(QuestionFileDto question) {
            return new Question(question.Tags, question.Answers.Select(answer => AnswerFileDto.Map(answer))) {
                //Id = question.Id,
                Author = question.Author,
                CreateDate = question.CreateDate,
                ChangeDate = question.ChangeDate,
                Topic = question.Topic,
                Text = question.Text
            };
        }
    }
}
