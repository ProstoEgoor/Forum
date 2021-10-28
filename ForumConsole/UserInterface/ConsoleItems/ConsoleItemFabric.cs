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
            ShowableConsoleItem mainItem = new ShowableConsoleItem(null, questionManager, questionManager.GetWrappedQuestions,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    consoleItem.Next = CreateQuestionShowItem(consoleItem, (consoleItem as ShowableConsoleItem).SelectedItem as QuestionWrapper);
                    consoleItem.Next.Reset();
                });

            return mainItem;
        }

        public static ConsoleItem CreateQuestionShowItem(ConsoleItem mainItem, QuestionWrapper question) {
            Menu menu = new Menu(new MenuItem[] {
                MenuItemFabric.CreateToAnswerMI("Ответить", ConsoleKey.F1),
                MenuItemFabric.CreateEscapeMI("Назад")
            });
            ShowableConsoleItem showQuestion = new ShowableConsoleItem(mainItem, question, question.GetWrappedAnswers,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    return;
                });

            return showQuestion;
        }
    }
}
