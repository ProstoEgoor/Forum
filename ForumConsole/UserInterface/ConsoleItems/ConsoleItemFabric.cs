using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public static class ConsoleItemFabric {
        public static ConsoleItem CreateMainItem(QuestionManagerWrapper questionManagerWrapper) {
            ListConsoleItem<string, QuestionWrapper> mainItem = new ListConsoleItem<string, QuestionWrapper>(null, "Вопросы:", questionManagerWrapper.GetWrappedQuestions,
                delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                    consoleItem.Next = CreateQuestionShowItem(consoleItem, (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.SelectedItem, questionManagerWrapper);
                    consoleItem.OnPause();
                }, delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                    questionManagerWrapper.Remove((consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.SelectedItem.Question);
                    (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
                });

            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Задать вопрос", ConsoleKey.F1));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateFindPropertyMI(ConsoleKey.F2));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateFindMI(ConsoleKey.F3));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateShowTagsMI(ConsoleKey.F4));
            mainItem.Menu.AddMenuItem(MenuItemFabric.CreateShowFileMi(ConsoleKey.F5));

            mainItem.EventHandler.AddHandler("WriteQuestion", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                consoleItem.Next = CreateWriteQuestion(consoleItem, new QuestionWrapper(), questionManagerWrapper);
                consoleItem.OnPause();
            });

            mainItem.EventHandler.AddHandler("FindOn", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                questionManagerWrapper.Find = true;
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
            });

            mainItem.EventHandler.AddHandler("FindOff", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                questionManagerWrapper.Find = false;
                (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
            });

            mainItem.EventHandler.AddHandler("WriteFieldEnd", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                if (e is ConsoleWriteEventArgs writeEvent) {
                    if (writeEvent.Tag == "FindText" && writeEvent.FieldType.Equals(typeof(string))) {
                        questionManagerWrapper.FindText = (writeEvent.Field as string) ?? "";
                        (consoleItem as ListConsoleItem<string, QuestionWrapper>).SelectFromList.UpdateList();
                    } else if (writeEvent.Tag == "FindTags" && writeEvent.FieldType.Equals(typeof(string))) {
                        questionManagerWrapper.FindTags.Clear();
                        if (writeEvent.Field is string tags && tags != null) {
                            questionManagerWrapper.FindTags.AddRange((writeEvent.Field as string).Split(' ', StringSplitOptions.RemoveEmptyEntries));
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

            mainItem.EventHandler.AddHandler("ShowTagsFrequency", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                consoleItem.Next = CreateShowTagsItem(consoleItem, new TagManagerWrapper(questionManagerWrapper.QuestionManager.TagManager));
                consoleItem.OnPause();
            });

            mainItem.EventHandler.AddHandler("ShowFileLoader", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                consoleItem.Next = CreateFileItem(consoleItem, questionManagerWrapper);
                consoleItem.OnPause();
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
                }) {
                Briefly = false
            };

            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateAskMI("Изменить вопрос", ConsoleKey.F1));
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateToAnswerMI("Ответить", ConsoleKey.F2));
            showQuestion.Menu.AddMenuItem(MenuItemFabric.CreateSortMI(ConsoleKey.F3));

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

            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateToAnswerMI("Изменить ответ", ConsoleKey.F1));

            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateVoteMI(true));
            showAnswer.Menu.AddMenuItem(MenuItemFabric.CreateVoteMI(false));

            showAnswer.EventHandler.AddHandler("WriteAnswer", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Next = CreateWriteAnswer(consoleItem, answerWrapper, questionWrapper);
                consoleItem.OnPause();
            });

            showAnswer.EventHandler.AddHandler("VotePos", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                answerWrapper.Answer.Vote(true);
            });

            showAnswer.EventHandler.AddHandler("VoteNeg", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                answerWrapper.Answer.Vote(false);
            });

            return showAnswer;
        }

        public static ConsoleItem CreateShowTagsItem(ConsoleItem prev, TagManagerWrapper tagManagerWrapper) {
            ListConsoleItem<TagManagerWrapper, string> showTags = new ListConsoleItem<TagManagerWrapper, string>(prev, tagManagerWrapper, tagManagerWrapper.GetWrappedTags, null, null);
            showTags.SelectFromList.Selectable = false;
            showTags.SelectFromList.Changeable = false;
            showTags.SelectFromList.UpdateAlways = true;

            showTags.EventHandler.AddHandler("UpdateView", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                tagManagerWrapper.TagWidth = Console.WindowWidth / 2;
            });

            return showTags;
        }

        public static ConsoleItem CreateFileItem(ConsoleItem prev, QuestionManagerWrapper questionManagerWrapper) {
            WriteFileConsoleItem writeFile = new WriteFileConsoleItem(prev, "Сохранение/Загрузка", questionManagerWrapper);

            return writeFile;
        }
    }
}
