using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;

namespace ForumConsole.ModelWrapper {
    public class QuestionManagerWrapper : IConsoleEditableContainer<Question> {
        public QuestionManager QuestionManager { get; }

        public QuestionManagerWrapper(QuestionManager questionManager) {
            QuestionManager = questionManager;
        }

        public IReadOnlyList<QuestionWrapper> GetWrappedQuestions() {
            return QuestionManager.Questions.Select(item => new QuestionWrapper(item)).ToList();
        }

        public void Show(int width, int indent, bool briefly) {
            Console.Write(new string(' ', indent));
            Console.WriteLine("Список вопросов:");
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
