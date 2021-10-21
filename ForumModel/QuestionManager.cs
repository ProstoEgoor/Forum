using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class QuestionManager {
        List<Question> ListQuestion { get; } = new List<Question>();

        TagManager TagManager { get; }

        public IEnumerable<Question> Questions => ListQuestion;
        public int Count => ListQuestion.Count; 

        public QuestionManager(TagManager tagManager) {
            TagManager = tagManager;
        }

        public QuestionManager(TagManager tagManager, IEnumerable<Question> questions) {
            TagManager = tagManager;

            foreach (var question in questions) {
                AddQuestion(question);
            }
        }

        public Question this[int index] => ListQuestion[index];

        public void AddQuestion(Question question) {
            ListQuestion.Add(question);
            TagManager.UpdateTags(question.Tags);
        }

        public bool RemoveQuestion(Question question) {
            return ListQuestion.Remove(question);
        }
    }
}
