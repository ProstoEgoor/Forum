using System;

namespace ForumConsole.UserInterface {
    public class MenuItem : IConsoleDisplayable {
        public ConsoleKeyInfo KeyInfo { get; }
        public string KeyTitle { get; }
        public string Description { get; }
        public int Order { get; }

        public MenuItem(ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) {
            KeyInfo = keyInfo;
            KeyTitle = keyTitle;
            Description = description;
            Order = order;
        }
        public MenuItem(ConsoleKeyInfo keyInfo, string description, int order = 1) : this(keyInfo, keyInfo.Key.ToString(), description, order) { }

        public virtual void Show(int width, int indent = 0, bool briefly = false) {
            int length = KeyTitle.Length + 1 + Description.Length;
            if (width - Console.CursorLeft < length) {
                Console.WriteLine(new string(' ', width - Console.CursorLeft));
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(KeyTitle);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" " + Description);
        }
    }
}