using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ForumModel;
using System.Linq;

namespace ForumConsole.FIles {
    [XmlType(TypeName = "Questions")]
    public class QuestionManagerFileDto {
        public QuestionFileDto[] Questions { get; set; }

        public static QuestionManagerFileDto Map(QuestionManager questionManager) {
            return new QuestionManagerFileDto() {
                Questions = questionManager.Questions.Select(question => QuestionFileDto.Map(question)).ToArray()
            };
        }

        public static QuestionManager Map(QuestionManagerFileDto questionManager, TagManager tagManager) {
            return new QuestionManager(tagManager, questionManager.Questions.Select(question => QuestionFileDto.Map(question)));
        }
    }
}
