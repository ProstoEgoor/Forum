using System.Collections.Generic;
using ForumConsole.UserInterface;
using ForumModel;
using System.Linq;
using System;

namespace ForumConsole {
    public class TagManagerWrapper : IConsoleDisplayable {
        public int TagWidth { get; set; } = 20;
        public TagManager TagManager { get; private set; }

        public bool CursorVisible => false;

        public (int top, int left) Cursor { get => (0, 0); set { } }

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public virtual ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public virtual ConsoleColor Background { get => background; set => background = value; }

        public TagManagerWrapper(TagManager tagManager) {
            TagManager = tagManager;
        }

        public IReadOnlyList<string> GetWrappedTags() {
            return TagManager.TagFrequencies.OrderByDescending(tag => tag.frequency).Select(item => string.Format(($"{{0,-{TagWidth}}} {{1}}"), item.tag, item.frequency)).ToList();
        }

        public void Show((int left, int right) indent) {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            int start = -1;
            int width = Console.WindowWidth - indent.left - indent.right;
            string str = string.Format(($"{{0,-{TagWidth}}} {{1}}"), "Тег", "частота");

            while (PrintHelper.TryGetLine(str, width, ref start, out string line)) {
                Console.Write(new string(' ', indent.left));
                Console.Write(line);
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }
        }
    }
}
