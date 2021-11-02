using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;

namespace ForumConsole.ModelWrapper {
    public class QuestionManagerWrapper : IConsoleEditableContainer<Question> {
        public QuestionManager QuestionManager { get; }

        public bool Find { get; set; }
        public string FindText { get; set; } = "";
        public List<string> FindTags { get; } = new List<string>();

        public QuestionManagerWrapper(QuestionManager questionManager) {
            QuestionManager = questionManager;
        }

        public IReadOnlyList<QuestionWrapper> GetWrappedQuestions() {
            if (Find) {
                return QuestionManager.GetFilteredQuestions(FindText, FindTags).Select(question => new QuestionWrapper(question)).ToList();
            } else {
                return QuestionManager.Questions.Select(question => new QuestionWrapper(question)).ToList();
            }
        }

        public void Add(Question item) {
            QuestionManager.AddQuestion(item);
        }

        public bool Remove(Question item) {
            return QuestionManager.RemoveQuestion(item);
        }

        public bool Replace(Question oldItem, Question newItem) {
            newItem.AddAnswer(oldItem.Answers.ToArray());
            return QuestionManager.ReplaceQuestion(oldItem, newItem);
        }
    }
}
