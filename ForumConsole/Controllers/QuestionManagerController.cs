using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.Validation;

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
            string author = InputValidator.ReadAuthorQ();
            /*DateTime date = DateTime.Now;*/
            string[] tags = InputValidator.ReadTags();
            string topic = InputValidator.ReadTopic();
            string text = InputValidator.ReadQuestionMsg();

            Question question = new Question(tags) {
                Author = author,
                Topic = topic,
                Text = text
            };

            QuestionManager.AddQuestion(question);

            return question;
        }

        public Answer FillAnswer(Question question) {
            string author = InputValidator.ReadAuthorA();
            /*DateTime date = DateTime.Now;*/
            string text = InputValidator.ReadAnswerMsg();

            Answer answer = new Answer() {
                Author = author,
                Text = text
            };

            question.AddAnswer(answer);

            return answer;
        }
    }
}
