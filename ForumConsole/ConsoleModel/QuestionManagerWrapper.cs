using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;

namespace ForumConsole.ConsoleModel {
    public class QuestionManagerWrapper : IConsoleEditable {
        public QuestionManager QuestionManager { get; }
        public QuestionManagerWrapper(QuestionManager questionManager) {
            QuestionManager = questionManager;
        }

        public IReadOnlyList<QuestionWrapper> GetWrappedQuestions() {
            return QuestionManager.Questions.Select(item => new QuestionWrapper(item)).ToList();
        }
    }
}
