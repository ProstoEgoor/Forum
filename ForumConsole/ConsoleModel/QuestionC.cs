using System.Collections.Generic;
using ForumConsole.UserInterface;
using System.Text;
using System;
using System.Collections;
using ForumModel;

namespace ForumConsole.ConsoleModel {
    public class QuestionC : Question, IConsolePrintable, IConsoleEditable {
        public QuestionC() : base() { }
        public QuestionC(IEnumerable<string> tags) : base(tags) { }

        public void Print(int width, int indent = 0, bool briefly = true) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Тема:\t{Topic}\n\r");
            buffer.Append($"Теги:\t{string.Join(", ", Tags)}\n\r");
            buffer.Append($"Автор:\t{Author}\n\r");
            buffer.Append($"Дата:\t{Date}\n\r");
            buffer.Append($"{Answers.Count} {PrintHelper.GetNumAddition(Answers.Count, "Ответ", "Ответа", "Ответов")}\n\r");
            string str = buffer.ToString();

            (int start, int end) boundaries = (0, 0);

            while (PrintHelper.TryGetLineBoundaries(str, boundaries.end, width - indent, out boundaries)) {
                Console.Write(new String(' ', indent));
                Console.WriteLine(str.Substring(boundaries.start, boundaries.end - boundaries.start));
            }

            boundaries = (0, 0);
            for (int i = 0; (!briefly || i < 2) && PrintHelper.TryGetLineBoundaries(Text, boundaries.end, width - indent - 1, out boundaries); i++) {
                Console.Write(new String(' ', indent + 1));
                Console.WriteLine(Text.Substring(boundaries.start, boundaries.end - boundaries.start));
            }

            if (briefly && PrintHelper.TryGetLineBoundaries(Text, boundaries.end, width - indent - 4, out boundaries)) {
                Console.Write(new String(' ', indent + 1));
                string tmp = Text.Substring(boundaries.start, boundaries.end - boundaries.start);
                Console.Write(Text.Substring(boundaries.start, boundaries.end - boundaries.start));
                Console.WriteLine("...");
            }
        }

    }
}
