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
            ListConsoleItem<string, QuestionWrapper> mainItem = new ListConsoleItem<string, QuestionWrapper>(null, "Вопросы:", questionManager.GetWrappedQuestions,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                    consoleItem.Next = CreateQuestionShowItem(consoleItem, (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.SelectedItem, questionManager);
                    consoleItem.OnPause();
                }, delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                    questionManager.Remove((consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.SelectedItem.Question);
                    (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
                });

            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Задать вопрос", ConsoleKey.F1));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateFindMI());
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateFindPropertyMI());

            mainItem.EventHandler.AddHandler("WriteQuestion", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                consoleItem.Next = CreateWriteQuestion(consoleItem, new QuestionWrapper(), questionManager);
                consoleItem.OnPause();
            });

            mainItem.EventHandler.AddHandler("FindOn", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                questionManager.Find = true;
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
            });

            mainItem.EventHandler.AddHandler("FindOff", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                questionManager.Find = false;
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
            });

            mainItem.EventHandler.AddHandler("WriteFieldEnd", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                if (e is ConsoleWriteEventArgs writeEvent) {
                    if (writeEvent.Tag == "FindText" && writeEvent.FieldType.Equals(typeof(string))) {
                        questionManager.FindText = (writeEvent.Field as string) ?? "";
                        (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
                    } else if (writeEvent.Tag == "FindTags" && writeEvent.FieldType.Equals(typeof(string))) {
                        questionManager.FindTags.Clear();
                        if (writeEvent.Field is string tags && tags != null) {
                            questionManager.FindTags.AddRange((writeEvent.Field as string).Split(' ', StringSplitOptions.RemoveEmptyEntries));
                        }
                        (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
                    }
                }
            });

            mainItem.EventHandler.AddHandler("InputFindStart", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.Changeable = false;
            });

            mainItem.EventHandler.AddHandler("InputFindEnd", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.Changeable = true;
            });


            return mainItem;
        }

        public static ConsoleItem CreateQuestionShowItem(ConsoleItem prev, QuestionWrapper question, QuestionManagerWrapper questionManager) {
            ListConsoleItem<QuestionWrapper, AnswerWrapper> showQuestion = new ListConsoleItem<QuestionWrapper, AnswerWrapper>(prev, question, question.GetWrappedAnswers,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    consoleItem.Next = CreateAnswerShowItem(consoleItem, (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.SelectedItem, question);
                    consoleItem.OnPause();
                }, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                    question.Remove((consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.SelectedItem.Answer);
                    (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.UpdateList();
                });
            showQuestion.Briefly = false;
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Изменить вопрос", ConsoleKey.F1));
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateToAnswerMI("Ответить", ConsoleKey.F2));
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateSortMI());

            showQuestion.EventHandler.AddHandler("WriteQuestion", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteQuestion(consoleItem, question, questionManager);
                consoleItem.OnPause();
            });

            showQuestion.EventHandler.AddHandler("WriteAnswer", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteAnswer(consoleItem, new AnswerWrapper(), question);
                consoleItem.OnPause();
            });

            showQuestion.EventHandler.AddHandler("SortAnswersOff", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                question.Sort = false;
                (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.UpdateList();
            });

            showQuestion.EventHandler.AddHandler("SortAnswerDateByAscending", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                question.Sort = true;
                question.SortDate = true;
                question.SortDateByAscending = true;
                (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.UpdateList();
            });

            showQuestion.EventHandler.AddHandler("SortAnswerDateByDescending", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                question.Sort = true;
                question.SortDate = true;
                question.SortDateByAscending = false;
                (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.UpdateList();
            });

            showQuestion.EventHandler.AddHandler("SortAnswerRatingByDescending", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                question.Sort = true;
                question.SortDate = false;
                (consoleItem as ListConsoleItem<QuestionWrapper, AnswerWrapper>).SelectFromList.UpdateList();
            });

            return showQuestion;
        }

        public static ConsoleItem CreateWriteQuestion(ConsoleItem prev, QuestionWrapper questionWrapper, QuestionManagerWrapper questionManagerWrapper) {
            string title = (questionWrapper.Question == null) ? "Создание вопроса:" : "Редактирование вопроса:";
            WriteConsoleItem<string, Question> writeQuestion = new WriteConsoleItem<string, Question>(prev, title, questionWrapper, questionManagerWrapper);

            writeQuestion.Menu.AddMenuItem(MenuItemFabric.CreateSaveMi("Сохранить вопрос", ConsoleKey.F1));

            return writeQuestion;
        }

        public static ConsoleItem CreateWriteAnswer(ConsoleItem prev, AnswerWrapper answerWrapper, QuestionWrapper questionWrapper) {
            string title = (questionWrapper.Question == null) ? "Создание ответа:" : "Редактирование ответа:";
            WriteConsoleItem<string, Answer> writeQuestion = new WriteConsoleItem<string, Answer>(prev, title, answerWrapper, questionWrapper);

            writeQuestion.Menu.AddMenuItem(MenuItemFabric.CreateSaveMi("Сохранить ответ", ConsoleKey.F1));

            return writeQuestion;
        }

        public static ConsoleItem CreateAnswerShowItem(ConsoleItem prev, AnswerWrapper answerWrapper, QuestionWrapper questionWrapper) {
            EntitledConsoleItem<AnswerWrapper> showAnswer = new EntitledConsoleItem<AnswerWrapper>(prev, answerWrapper);

            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateToAnswerMI("Изменить вопрос", ConsoleKey.F1));

            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateVoteMI(true));
            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateVoteMI(false));

            showAnswer.EventHandler.AddHandler("WriteAnswer", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteAnswer(consoleItem, answerWrapper, questionWrapper);
                consoleItem.OnPause();
            });

            showAnswer.EventHandler.AddHandler("VotePos", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                answerWrapper.Answer.Vote(1);
            });

            showAnswer.EventHandler.AddHandler("VoteNeg", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                answerWrapper.Answer.Vote(-1);
            });

            return showAnswer;
        }
    }
}
