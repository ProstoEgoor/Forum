using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;

namespace ForumConsole.ConsoleModel {
    public class QuestionManagerC : QuestionManager, IConsoleEditable {
        public QuestionManagerC(TagManager tagManager) : base(tagManager) {
        }

        public QuestionManagerC(TagManager tagManager, IEnumerable<Question> questions) : base(tagManager, questions) {
        }
    }
}
