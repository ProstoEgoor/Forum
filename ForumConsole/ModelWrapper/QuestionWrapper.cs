using System.Collections.Generic;
using ForumConsole.UserInterface;
using System.Text;
using System;
using System.Collections;
using ForumModel;
using System.Linq;

namespace ForumConsole.ModelWrapper {
    public class QuestionWrapper : IConsoleDisplayable, IConsoleEditable<Question>, IConsoleEditableContainer<Answer> {
        public Question Question { get; set; }

        public IReadOnlyList<WriteField> GetWriteFields {
            get {
                List<WriteField> writeFields = new List<WriteField>();
                writeFields.Add(new WriteField<string>(true, "Тема", Question?.Topic ?? "", (field) => field, (field) => field.Trim().Length > 0, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space }));
                writeFields.Add(new WriteField<string>(true, "Теги", (Question == null) ? "" : string.Join(" ", Question.Tags), (field) => field, (field) => true, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space })); ;
                writeFields.Add(new WriteField<string>(true, "Автор", Question?.Author ?? "", (field) => field, (field) => field.Trim().Length > 0, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space }));
                writeFields.Add(new ReactiveWriteField<DateTime>("Дата", (Question == null) ? DateTime.Now.ToString() : Question.Date.ToString(), () => DateTime.Now, (field) => DateTime.Parse(field), (field) => DateTime.TryParse(field, out _)));
                writeFields.Add(new WriteField<string>(true, "Текст", Question?.Text ?? "", (field) => field, (field) => field.Trim().Length > 0, new CharType[] { CharType.letter, CharType.digit, CharType.punctuation, CharType.space, CharType.lineSeparator }));

                return writeFields;
            }
        }

        Question IConsoleEditable<Question>.Element { get => Question; set => Question = value; }

        public QuestionWrapper() { }

        public QuestionWrapper(Question question) {
            Question = question;
        }

        public IReadOnlyList<AnswerWrapper> GetWrappedAnswers() {
            return Question.Answers.Select(item => new AnswerWrapper(item)).ToList();
        }

        public void Show(int width, int indent = 0, bool briefly = true) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Тема:\t{Question.Topic}\r\n");
            buffer.Append($"Теги:\t{string.Join(", ", Question.Tags)}\r\n");
            buffer.Append($"Автор:\t{Question.Author}\r\n");
            buffer.Append($"Дата:\t{Question.Date}\r\n");
            buffer.Append($"{Question.Answers.Count} {PrintHelper.GetNumAddition(Question.Answers.Count, "Ответ", "Ответа", "Ответов")}\r\n");
            string str = buffer.ToString();

            int start = -1;
            string line;

            while (PrintHelper.TryGetLine(str, width - indent, ref start, out line)) {
                Console.Write(new string(' ', indent));
                Console.WriteLine(line);
            }

            start = 0;
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLine(Question.Text, width - indent - 1, ref start, out line); i++) {
                Console.Write(new string(' ', indent + 1));
                Console.WriteLine(line);
            }

            if (briefly && PrintHelper.TryGetLine(Question.Text, width - indent - 4, ref start, out line)) {
                Console.Write(new string(' ', indent + 1));
                Console.Write(line);
                Console.WriteLine("...");
            }
        }

        public Question CreateFromWriteFields(IReadOnlyList<WriteField> writeFields) {
            string topic = (writeFields[0] as WriteField<string>).ParseField;
            string[] tags = (writeFields[1] as WriteField<string>).ParseField.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string author = (writeFields[2] as WriteField<string>).ParseField;
            DateTime date = (writeFields[3] as WriteField<DateTime>).ParseField;
            string text = (writeFields[4] as WriteField<string>).ParseField;

            return new Question(tags) {
                Author = author,
                Date = date,
                Topic = topic,
                Text = text
            };
        }

        public void Add(Answer item) {
            Question.AddAnswer(item);
        }

        public bool Remove(Answer item) {
            throw new NotImplementedException();
        }

        public bool Replace(Answer oldItem, Answer newItem) {
            throw new NotImplementedException();
        }
    }
}
