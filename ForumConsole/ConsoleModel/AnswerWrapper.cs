using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole.ConsoleModel {
    public class AnswerWrapper : IConsoleDisplayable {
        public Answer Answer { get; }
        public AnswerWrapper(Answer answer) {
            Answer = answer;
        }

        public void Show(int width, int indent = 0, bool briefly = false) {
            briefly = false;

            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Автор:\t{Answer.Author}\n\r");
            buffer.Append($"Дата:\t{Answer.Date}\n\r");
            buffer.Append($"Рейтинг:\t{Answer.Rating}\n\r");
            string str = buffer.ToString();

            (int start, int end) boundaries = (0, 0);

            while (PrintHelper.TryGetLineBoundaries(str, boundaries.end, width - indent, out boundaries)) {
                Console.Write(new String(' ', indent));
                Console.WriteLine(str[boundaries.start..boundaries.end]);
            }

            boundaries = (0, 0);
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLineBoundaries(Answer.Text, boundaries.end, width - indent - 1, out boundaries); i++) {
                Console.Write(new String(' ', indent + 1));
                Console.WriteLine(Answer.Text[boundaries.start..boundaries.end]);
            }

            if (briefly && PrintHelper.TryGetLineBoundaries(Answer.Text, boundaries.end, width - indent - 4, out boundaries)) {
                Console.Write(new String(' ', indent + 1));
                Console.Write(Answer.Text[boundaries.start..boundaries.end]);
                Console.WriteLine("...");
            }
        }
    }
}
