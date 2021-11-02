using System;

namespace ForumConsole.UserInterface {
    public class MenuItem : IConsoleDisplayable {
        public ConsoleKeyInfo KeyInfo { get; set; }
        public string KeyTitle { get; set; }
        public string Description { get; set; }
        public int Order { get; }

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public virtual ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public virtual ConsoleColor Background { get => background; set => background = value; }

        public ConsoleColor KeyColor { get; set; } = ConsoleColor.Yellow;

        public virtual bool CursorVisible => false;

        public (int top, int left) Cursor { get; set; } = (0, 0);

        public MenuItem(ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) {
            KeyInfo = keyInfo;
            KeyTitle = keyTitle;
            Description = description;
            Order = order;
        }
        public MenuItem(ConsoleKeyInfo keyInfo, string description, int order = 1) : this(keyInfo, keyInfo.Key.ToString(), description, order) { }

        public virtual void Show((int left, int right) indent) {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = Foreground;

            int length = KeyTitle.Length + 1 + Description.Length;
            if (Console.WindowWidth - indent.right - Console.CursorLeft < length) {
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }
            Console.ForegroundColor = KeyColor;
            Console.Write(KeyTitle);
            Console.ForegroundColor = Foreground;
            Console.Write(" " + Description);
        }
    }
}