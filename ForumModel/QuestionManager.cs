using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class QuestionManager {
        protected List<Question> ListQuestion { get; } = new List<Question>();

        TagManager TagManager { get; }

        public IReadOnlyList<Question> Questions => ListQuestion;

        public QuestionManager(TagManager tagManager) {
            TagManager = tagManager;
        }

        public QuestionManager(TagManager tagManager, IEnumerable<Question> questions) {
            TagManager = tagManager;

            foreach (var question in questions) {
                AddQuestion(question);
            }
        }

        public void AddQuestion(Question question) {
            ListQuestion.Add(question);
            TagManager.UpdateTags(question.Tags);
        }

        public bool RemoveQuestion(Question question) {
            return ListQuestion.Remove(question);
        }
    }
}
