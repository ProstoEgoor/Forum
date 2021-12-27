using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole.ModelWrapper {
    public class AnswerWrapper : IConsoleDisplayableBriefly, IConsoleEditable<Answer> {
        public Answer Answer { get; set; }

        public IReadOnlyList<WriteField> GetWriteFields {
            get {
                List<WriteField> writeFields = new List<WriteField> {
                    new WriteField<string>(true, "AnswerAuthor", "Автор", Answer.Author, (field) => field, (field) => field.Trim().Length > 0, (int)CharType.All ^ (int)CharType.LineSeparator),
                    //new ReactiveWriteField<DateTime>("Дата", Answer.Date.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)),
                    //new WriteField<string>(true, "AnswerText", "Текст", Answer.Text, (field) => field, (field) => field.Trim().Length > 0, (int)CharType.All)
                };

                /*if (isEmpty) {
                    writeFields.Add(new ReactiveWriteField<DateTime>("Дата создания", Answer.CreateDate.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)));
                } else {
                    writeFields.Add(new WriteField<DateTime>(false, "", "Дата создания", Answer.CreateDate.ToString(), (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _), (int)CharType.All));
                }
                writeFields.Add(new ReactiveWriteField<DateTime>("Дата редактирования", Answer.ChangeDate.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)));*/

                writeFields.Add(new WriteField<string>(true, "AnswerText", "Текст", Answer.Text, (field) => field, (field) => field.Trim().Length > 0, (int)CharType.All));

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

        public Answer Element {
            get => Answer;
            set {
                if (value != null) {
                    isEmpty = false;
                    Answer = value;
                }
            }
        }

        public AnswerWrapper() {
            isEmpty = true;
            Answer = new Answer();
        }
        public AnswerWrapper(Answer answer) {
            Answer = answer;
        }

        public void Show((int left, int right) indent, bool briefly) {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Автор: {Answer.Author}\r\n");
            buffer.Append($"Дата создания: {Answer.CreateDate}\r\n");
            buffer.Append($"Дата редактирования: {Answer.ChangeDate}\r\n");
            buffer.Append($"Рейтинг: {Answer.Rating}\r\n");
            string str = buffer.ToString();

            int start = -1;
            string line;
            int width = Console.WindowWidth - indent.left - indent.right;

            while (PrintHelper.TryGetLine(str, width, ref start, out line)) {
                Console.Write(new string(' ', indent.left));
                Console.Write(line);
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            start = 0;
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLine(Answer.Text, width - 1, ref start, out line); i++) {
                Console.Write(new string(' ', indent.left + 1));
                Console.Write(line);
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            if (briefly && PrintHelper.TryGetLine(Answer.Text, width - 4, ref start, out line)) {
                Console.Write(new string(' ', indent.left + 1));
                Console.Write(line);
                Console.Write("...");
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            Console.WriteLine();
        }

        public Answer CreateFromWriteFields(IReadOnlyList<WriteField> writeFields) {
            string author = (writeFields[0] as WriteField<string>).ParseField;
            DateTime createDate = Answer.CreateDate;
            DateTime changeDate = DateTime.Now;
            string text = (writeFields[2] as WriteField<string>).ParseField;
            int votePositive = Answer.VotePositive;
            int voteNegative = Answer.VoteNegative;

            var answer = new Answer(votePositive, voteNegative) {
                Author = author,
                CreateDate = createDate,
                ChangeDate = changeDate,
                Text = text,
            };

            if (!IsEmpty) {
                answer.Id = Element.Id;
            }

            return answer;
        }

        public void Show((int left, int right) indent) {
            Show(indent, false);
        }
    }
}
