using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface {
    public static class ConsoleItemFabric {
        public static ConsoleItem CreateMainItem(QuestionManagerWrapper questionManager) {
            Menu menu = new Menu(new MenuItem[] {
                MenuItemFabric.CreateAskMI("Задать вопрос", ConsoleKey.F1),
                MenuItemFabric.CreateEscapeMI("Выйти")
            });
            ShowableConsoleItem<QuestionWrapper> mainItem = new ShowableConsoleItem<QuestionWrapper>(null, questionManager, questionManager.GetWrappedQuestions,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    consoleItem.Next = CreateQuestionShowItem(consoleItem, (consoleItem as ShowableConsoleItem<QuestionWrapper>).SelectFromList.SelectedItem);
                    consoleItem.OnPause();
                });

            return mainItem;
        }

        public static ConsoleItem CreateQuestionShowItem(ConsoleItem mainItem, QuestionWrapper question) {
            Menu menu = new Menu(new MenuItem[] {
                MenuItemFabric.CreateToAnswerMI("Ответить", ConsoleKey.F1),
                MenuItemFabric.CreateEscapeMI("Назад")
            });
            ShowableConsoleItem<AnswerWrapper> showQuestion = new ShowableConsoleItem<AnswerWrapper>(mainItem, question, question.GetWrappedAnswers,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    return;
                });

            return showQuestion;
        }
    }
}
