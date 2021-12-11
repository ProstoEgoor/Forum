using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ForumModel;

namespace ForumConsole.FIles {
    [XmlType(TypeName = "Answer")]
    public class AnswerFileDto {

        public int? Id { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public uint NumberOfVotes { get; set; }

        public static AnswerFileDto Map(Answer answer) {
            return new AnswerFileDto() {
                Id = answer.Id,
                Author = answer.Author,
                Date = answer.Date,
                Text = answer.Text,
                Rating = answer.Rating,
                NumberOfVotes = answer.NumberOfVotes
            };
        }

        public static Answer Map(AnswerFileDto answer) {
            return new Answer(answer.Rating, answer.NumberOfVotes) {
                Id = answer.Id,
                Author = answer.Author,
                Date = answer.Date,
                Text = answer.Text
            };
        }
    }
}
