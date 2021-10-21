using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;

namespace ForumConsole {
    class QuestionManagerController { 
        public QuestionManager QuestionManager { get; }

        public QuestionManagerController(TagManager tagManager) {
            QuestionManager = new QuestionManager(tagManager);
        }

        public QuestionManagerController(TagManager tagManager, IEnumerable<Question> questions) {
            QuestionManager = new QuestionManager(tagManager, questions);
        }

        public Question FillQuestion() {
            //TODO input question's fields
            string author = "";
            /*DateTime date = DateTime.Now;*/
            string[] tags = new string[0];
            string topic = "";
            string text = "";

            Question question = new Question(tags) {
                Author = author,
                Topic = topic,
                Text = text
            };

            QuestionManager.AddQuestion(question);

            return question;
        }

        public Answer FillAnswer(Question question) {
            //TODO input answer's fields
            string author = "";
            /*DateTime date = DateTime.Now;*/
            string text = "";

            Answer answer = new Answer() {
                Author = author,
                Text = text
            };

            question.AddAnswer(answer);

            return answer;
        }
    }
}
