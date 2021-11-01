﻿using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole.ModelWrapper {
    public class AnswerWrapper : IConsoleDisplayable, IConsoleEditable<Answer> {
        public Answer Answer { get; set; }

        public IReadOnlyList<WriteField> GetWriteFields {
            get {
                List<WriteField> writeFields = new List<WriteField>();
                writeFields.Add(new WriteField<string>(true, "Автор", Answer?.Author ?? "", (field) => field, (field) => field.Trim().Length > 0, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space }));
                writeFields.Add(new ReactiveWriteField<DateTime>("Дата", (Answer == null) ? DateTime.Now.ToString() : Answer.Date.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)));
                writeFields.Add(new WriteField<int>(false, "Рейтинг", (Answer == null) ? "0" : Answer.Rating.ToString(), (field) => int.Parse(field), (field) => int.TryParse(field, out _), new CharType[] { }));
                writeFields.Add(new WriteField<string>(true, "Текст", Answer?.Text ?? "", (field) => field, (field) => field.Trim().Length > 0, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space, CharType.lineSeparator }));

                return writeFields;
            }
        }

        public Answer Element { get => Answer; set => Answer = value; }

        public AnswerWrapper(Answer answer) {
            Answer = answer;
        }

        public void Show(int width, int indent = 0, bool briefly = false) {
            briefly = false;

            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Автор:\t{Answer.Author}\r\n");
            buffer.Append($"Дата:\t{Answer.Date}\r\n");
            buffer.Append($"Рейтинг:\t{Answer.Rating}\r\n");
            string str = buffer.ToString();

            int start = -1;
            string line;

            while (PrintHelper.TryGetLine(str, width - indent, ref start, out line)) {
                Console.Write(new string(' ', indent));
                Console.WriteLine(line);
            }

            start = 0;
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLine(Answer.Text, width - indent - 1, ref start, out line); i++) {
                Console.Write(new string(' ', indent + 1));
                Console.WriteLine(line);
            }

            if (briefly && PrintHelper.TryGetLine(Answer.Text, width - indent - 4, ref start, out line)) {
                Console.Write(new string(' ', indent + 1));
                Console.Write(line);
                Console.WriteLine("...");
            }
        }

        public Answer CreateFromWriteFields(IReadOnlyList<WriteField> writeFields) {
            string author = (writeFields[0] as WriteField<string>).ParseField;
            DateTime date = (writeFields[1] as WriteField<DateTime>).ParseField;
            int rating = (writeFields[2] as WriteField<int>).ParseField;
            string text = (writeFields[3] as WriteField<string>).ParseField;
            uint numberOfVotes = Answer?.NumberOfVotes ?? 0;

            return new Answer(rating, numberOfVotes) {
                Author = author,
                Date = date,
                Text = text,
            };
        }
    }
}
