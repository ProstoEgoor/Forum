using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole.ConsoleModel {
    public class AnswerC : Answer, IConsolePrintable {
        public AnswerC() : base() { }
        public AnswerC(int rating, uint numberOfVotes = 0) : base(rating, numberOfVotes) { }

        public void Print(int width, int indent = 0, bool briefly = true) {
            briefly = false;

            StringBuilder buffer = new StringBuilder();
            buffer.Append($"Автор:\t{Author}\n\r");
            buffer.Append($"Дата:\t{Date}\n\r");
            buffer.Append($"Рейтинг:\t{Rating}\n\r");
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
                Console.Write(Text.Substring(boundaries.start, boundaries.end - boundaries.start));
                Console.WriteLine("...");
            }
        }
    }
}
