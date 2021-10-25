using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface {
    public static class ConsoleItemFabric {
        public static ConsoleItem CreateMainItem(QuestionManagerC questionManager) {
            Menu menu = new Menu(new MenuItem[] {
                MenuItemFabric.CreateAskMI("Задать вопрос", ConsoleKey.F1),
                MenuItemFabric.CreateEscapeMI("Выйти")
            });
            ShowableConsoleItem<Question> mainItem = new ShowableConsoleItem<Question>(null, menu, questionManager.Questions);
            mainItem.Selectable = true;

            mainItem.NextContent = delegate (int position) {
                return CreateQuestionShowItem(mainItem, questionManager.Questions[position]);
            };

            return mainItem;
        }

        public static ConsoleItem CreateQuestionShowItem(ConsoleItem mainItem, Question question) {
            Menu menu = new Menu(new MenuItem[] {
                MenuItemFabric.CreateToAnswerMI("Ответить", ConsoleKey.F1),
                MenuItemFabric.CreateEscapeMI("Назад")
            });
            ShowableConsoleItem<Answer> showQuestion = new ShowableConsoleItem<Answer>(mainItem, menu, question.Answers);

            return showQuestion;
        }
    }
}
