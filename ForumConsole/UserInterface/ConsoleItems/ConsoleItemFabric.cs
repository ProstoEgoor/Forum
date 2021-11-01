using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public static class ConsoleItemFabric {
        public static ConsoleItem CreateMainItem(QuestionManagerWrapper questionManager) {
            ListConsoleItem<QuestionManagerWrapper, QuestionWrapper> mainItem = new ListConsoleItem<QuestionManagerWrapper, QuestionWrapper>(null, questionManager, questionManager.GetWrappedQuestions,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    consoleItem.Next = CreateQuestionShowItem(consoleItem, (consoleItem as ListConsoleItem<QuestionManagerWrapper, QuestionWrapper>).SelectFromList.SelectedItem, questionManager);
                    consoleItem.OnPause();
                }, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    _ = questionManager.Remove((consoleItem as ListConsoleItem<QuestionManagerWrapper, QuestionWrapper>).SelectFromList.SelectedItem.Question);
                });

            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Задать вопрос", ConsoleKey.F1));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateTemp());

            mainItem.EventHandler.AddHandler(ConsoleEvent.WriteQuestion, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteQuestion(consoleItem, new QuestionWrapper(), questionManager);
                consoleItem.OnPause();
            });

            mainItem.EventHandler.AddHandler(ConsoleEvent.Idle, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                Console.Beep();
            });

            mainItem.SelectFromList.SelectedColor = ConsoleColor.Gray;
            return mainItem;
        }

        public static ConsoleItem CreateQuestionShowItem(ConsoleItem prev, QuestionWrapper question, QuestionManagerWrapper questionManager) {
            ListConsoleItem<QuestionWrapper, AnswerWrapper> showQuestion = new ListConsoleItem<QuestionWrapper, AnswerWrapper>(prev, question, question.GetWrappedAnswers,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    consoleItem.Next = CreateAnswerShowItem(consoleItem, (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.SelectedItem, question);
                    consoleItem.OnPause();
                }, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    return;
                });
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Изменить вопрос", ConsoleKey.F1));
            showQuestion.EventHandler.AddHandler(ConsoleEvent.WriteQuestion, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteQuestion(consoleItem, question, questionManager);
                consoleItem.OnPause();
            });

            return showQuestion;
        }

        public static ConsoleItem CreateWriteQuestion(ConsoleItem prev, QuestionWrapper questionWrapper, QuestionManagerWrapper questionManagerWrapper) {
            string title = (questionWrapper.Question == null) ? "Создание вопроса:" : "Редактирование вопроса:";
            WriteConsoleItem<string, Question> writeQuestion = new WriteConsoleItem<string, Question>(prev, title, questionWrapper, questionManagerWrapper);

            writeQuestion.Menu.AddMenuItem(MenuItemFabric.CreateSaveMi("Сохранить вопрос", ConsoleKey.F1));

            return writeQuestion;
        }

        public static ConsoleItem CreateAnswerShowItem(ConsoleItem prev, AnswerWrapper answerWrapper, QuestionWrapper questionWrapper) {
            EntitledConsoleItem<AnswerWrapper> showAnswer = new EntitledConsoleItem<AnswerWrapper>(prev, answerWrapper);

/*            showAnswer.Menu.AddMenuItem(new MenuItem(new ConsoleKeyInfo('+', ConsoleKey.OemPlus, false, false, false), ConsoleEvent.VotePos, "Голосовать за"));
            showAnswer.Menu.AddMenuItem(new MenuItem(new ConsoleKeyInfo('-', ConsoleKey.OemMinus, false, false, false), ConsoleEvent.VoteNeg, "Голосовать за"));

            showAnswer.EventHandler.AddHandler(ConsoleEvent.VotePos, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                ((consoleItem as EntitledConsoleItem).Content as AnswerWrapper).Answer.Vote(1);
            });

            showAnswer.EventHandler.AddHandler(ConsoleEvent.VoteNeg, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                ((consoleItem as EntitledConsoleItem).Content as AnswerWrapper).Answer.Vote(-1);
            });*/

            return showAnswer;
        }
    }
}
