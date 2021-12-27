using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ForumModel;

namespace ForumConsole.FIles {
    [XmlType(TypeName = "Answer")]
    public class AnswerFileDto {

        //public long? Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Text { get; set; }
        public int VotePositive { get; set; }
        public int VoteNegative { get; set; }

        public static AnswerFileDto Map(Answer answer) {
            return new AnswerFileDto() {
                //Id = answer.Id,
                Author = answer.Author,
                CreateDate = answer.CreateDate,
                ChangeDate = answer.ChangeDate,
                Text = answer.Text,
                VotePositive = answer.VotePositive,
                VoteNegative = answer.VoteNegative
            };
        }

        public static Answer Map(AnswerFileDto answer) {
            return new Answer(answer.VotePositive, answer.VoteNegative) {
                //Id = answer.Id,
                Author = answer.Author,
                CreateDate = answer.CreateDate,
                ChangeDate = answer.ChangeDate,
                Text = answer.Text
            };
        }
    }
}
