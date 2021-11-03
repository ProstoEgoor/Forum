using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ForumModel;
using System.Linq;

namespace ForumConsole.FIles {

    [XmlType(TypeName = "Question")]
    public class QuestionFileDto {
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string[] Tags { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public AnswerFileDto[] Answers { get; set; }

        public static QuestionFileDto Map(Question question) {
            return new QuestionFileDto() {
                Author = question.Author,
                Date = question.Date,
                Tags = question.Tags.ToArray(),
                Topic = question.Topic,
                Text = question.Text,
                Answers = question.Answers.Select(answer => AnswerFileDto.Map(answer)).ToArray(),
            };
        }

        public static Question Map(QuestionFileDto question) {
            return new Question(question.Tags, question.Answers.Select(answer => AnswerFileDto.Map(answer))) {
                Author = question.Author,
                Date = question.Date,
                Topic = question.Topic,
                Text = question.Text
            };
        }
    }
}
