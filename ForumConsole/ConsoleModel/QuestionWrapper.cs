using System.Collections.Generic;
using ForumConsole.UserInterface;
using System.Text;
using System;
using System.Collections;
using ForumModel;
using System.Linq;

namespace ForumConsole.ConsoleModel {
    public class QuestionWrapper : IConsoleDisplayable, IConsoleEditable {
        public Question Question { get; }
        public QuestionWrapper(Question question) {
            Question = question;
        }

        public IReadOnlyList<AnswerWrapper> GetWrappedAnswers() {
            return Question.Answers.Select(item => new AnswerWrapper(item)).ToList();
        }

        public void Show(int width, int indent = 0, bool briefly = true) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Тема:\t{Question.Topic}\n\r");
            buffer.Append($"Теги:\t{string.Join(", ", Question.Tags)}\n\r");
            buffer.Append($"Автор:\t{Question.Author}\n\r");
            buffer.Append($"Дата:\t{Question.Date}\n\r");
            buffer.Append($"{Question.Answers.Count} {PrintHelper.GetNumAddition(Question.Answers.Count, "Ответ", "Ответа", "Ответов")}\n\r");
            string str = buffer.ToString();

            (int start, int end) boundaries = (0, 0);

            while (PrintHelper.TryGetLineBoundaries(str, boundaries.end, width - indent, out boundaries)) {
                Console.Write(new String(' ', indent));
                Console.WriteLine(str[boundaries.start..boundaries.end]);
            }

            boundaries = (0, 0);
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLineBoundaries(Question.Text, boundaries.end, width - indent - 1, out boundaries); i++) {
                Console.Write(new String(' ', indent + 1));
                Console.WriteLine(Question.Text[boundaries.start..boundaries.end]);
            }

            if (briefly && PrintHelper.TryGetLineBoundaries(Question.Text, boundaries.end, width - indent - 4, out boundaries)) {
                Console.Write(new String(' ', indent + 1));
                Console.Write(Question.Text[boundaries.start..boundaries.end]);
                Console.WriteLine("...");
            }
        }

    }
}
