using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class QuestionManager {
        List<Question> ListQuestion { get; } = new List<Question>();

        TagManager tagManager;

        public QuestionManager(TagManager tagManager) {
            this.tagManager = tagManager;
        }

        public void AddQuestion(Question question) {
            ListQuestion.Add(question);
            tagManager.UpdateTags(question.Tags);
        }

        public bool RemoveQuestion(Question question) {
            return ListQuestion.Remove(question);
        }
    }
}
