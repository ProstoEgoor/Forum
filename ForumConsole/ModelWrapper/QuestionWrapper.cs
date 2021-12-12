using System.Collections.Generic;
using ForumConsole.UserInterface;
using System.Text;
using System;
using System.Collections;
using ForumModel;
using System.Linq;

namespace ForumConsole.ModelWrapper {
    public class QuestionWrapper : IConsoleDisplayableBriefly, IConsoleEditable<Question>, IConsoleEditableContainer<Answer> {
        public Question Question { get; private set; }

        public bool Sort { get; set; }
        public bool SortDate { get; set; }
        public bool SortDateByAscending { get; set; }

        public IReadOnlyList<WriteField> GetWriteFields {
            get {
                List<WriteField> writeFields = new List<WriteField> {
                    new WriteField<string>(true, "QuestionTopic", "Тема", Question.Topic, (field) => field, (field) => field.Trim().Length > 0, (int) CharType.All ^ (int) CharType.LineSeparator),
                    new WriteField<string>(true, "QuestionTags", "Теги", string.Join(" ", Question.Tags), (field) => field, (field) => true, (int) CharType.All ^ (int) CharType.LineSeparator),
                    new WriteField<string>(true, "QuestionAuthor", "Автор", Question.Author, (field) => field, (field) => field.Trim().Length > 0, (int) CharType.All ^ (int) CharType.LineSeparator),
                    //new ReactiveWriteField<DateTime>("Дата", Question.Date.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)),
                    //new WriteField<string>(true, "QuestionText", "Текст", Question.Text, (field) => field, (field) => field.Trim().Length > 0, (int) CharType.All)
                };

                if (isEmpty) {
                    writeFields.Add(new ReactiveWriteField<DateTime>("Дата создания", Question.Date.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)));
                } else {
                    writeFields.Add(new WriteField<DateTime>(false, "", "Дата создания", Question.Date.ToString(), (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _), (int) CharType.All));
                }

                writeFields.Add(new WriteField<string>(true, "QuestionText", "Текст", Question.Text, (field) => field, (field) => field.Trim().Length > 0, (int)CharType.All));

                return writeFields;
            }
        }

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public ConsoleColor Background { get => background; set => background = value; }

        public bool CursorVisible => false;

        public (int top, int left) Cursor { get; set; } = (0, 0);

        bool isEmpty;
        public bool IsEmpty => isEmpty;

        public Question Element {
            get => Question;
            set {
                if (value != null) {
                    isEmpty = false;
                    Question = value;
                }
            }
        }

        public QuestionWrapper() {
            isEmpty = true;
            Question = new Question();
        }

        public QuestionWrapper(Question question) {
            Question = question;
        }

        public IReadOnlyList<AnswerWrapper> GetWrappedAnswers() {
            if (Sort) {
                return Question.GetSortedAnswers(SortDate, SortDateByAscending).Select(answer => new AnswerWrapper(answer)).ToList();
            } else {
                return Question.Answers.Select(answer => new AnswerWrapper(answer)).ToList();
            }
        }

        public void Show((int left, int right) indent, bool briefly) {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Тема: {Question.Topic}\r\n");
            buffer.Append($"Теги: {string.Join(", ", Question.Tags)}\r\n");
            buffer.Append($"Автор: {Question.Author}\r\n");
            buffer.Append($"Дата создания: {Question.Date}\r\n");
            buffer.Append($"{Question.Answers.Count} {PrintHelper.GetNumAddition(Question.Answers.Count, "Ответ", "Ответа", "Ответов")}\r\n");
            string str = buffer.ToString();

            int start = -1;
            string line;
            int width = Console.WindowWidth - indent.left - indent.right;

            while (PrintHelper.TryGetLine(str, width, ref start, out line)) {
                Console.Write(new string(' ', indent.left));
                Console.Write(line);
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            start = 0;
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLine(Question.Text, width - 1, ref start, out line); i++) {
                Console.Write(new string(' ', indent.left + 1));
                Console.Write(line);
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            if (briefly && PrintHelper.TryGetLine(Question.Text, width - 4, ref start, out line)) {
                Console.Write(new string(' ', indent.left + 1));
                Console.Write(line);
                Console.Write("...");
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }
        }
        public void Show((int left, int right) indent) {
            Show(indent, false);
        }

        public Question CreateFromWriteFields(IReadOnlyList<WriteField> writeFields) {
            string topic = (writeFields[0] as WriteField<string>).ParseField;
            string[] tags = (writeFields[1] as WriteField<string>).ParseField.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string author = (writeFields[2] as WriteField<string>).ParseField;
            DateTime date = (writeFields[3] as WriteField<DateTime>).ParseField;
            string text = (writeFields[4] as WriteField<string>).ParseField;

            var question = new Question(tags) {
                Author = author,
                Date = date,
                Topic = topic,
                Text = text
            };

            if (!IsEmpty) {
                question.Id = Element.Id;
            }

            return question;
        }

        public void Add(Answer answer) {
            Question.AddAnswer(answer);
        }

        public bool Remove(Answer answer) {
            return Question.RemoveAnswer(answer);
        }

        public bool Replace(Answer oldAnswer, Answer newAnswer) {
            return Question.ReplaceAnswer(oldAnswer, newAnswer);
        } 
    }
}
