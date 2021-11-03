using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumModel {
    public class QuestionManager {
        protected List<Question> ListQuestion { get; } = new List<Question>();

        public TagManager TagManager { get; }

        public IReadOnlyList<Question> Questions => ListQuestion;

        public IReadOnlyList<Question> GetFilteredQuestions(string text, IEnumerable<string> tags) {
            return ListQuestion.FindAll(delegate (Question question) {
                bool textPrecidate = (text.Length == 0 || question.Text.Contains(text, StringComparison.OrdinalIgnoreCase));

                bool tagsPredicate = true;
                foreach (var tag in tags) {
                    if (tagsPredicate) {
                        tagsPredicate = question.ContainsTag(tag);
                    }
                }

                return textPrecidate && tagsPredicate;
            }).OrderByDescending(question => question.Answers.Count).ToList();
        }

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
            TagManager.UpdateTags(question.Tags, true);
            return ListQuestion.Remove(question);
        }

        public bool ReplaceQuestion(Question oldQuestion, Question newQuestion) {
            int position = ListQuestion.IndexOf(oldQuestion);
            if (position == -1)
                return false;
            TagManager.UpdateTags(oldQuestion.Tags, true);
            ListQuestion[position] = newQuestion;
            TagManager.UpdateTags(newQuestion.Tags);
            return true;
        }
    }
}
