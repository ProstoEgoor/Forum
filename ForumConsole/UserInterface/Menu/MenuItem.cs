using System;

namespace ForumConsole.UserInterface {
    public class MenuItem : IConsoleDisplayable {
        public ConsoleKeyInfo KeyInfo { get; }
        public ConsoleEvent ConsoleEvent { get; }
        public string Title { get; }
        public bool Selected { get; set; }

        public ConsoleColor SelectedColor { get; set; } = ConsoleColor.DarkBlue;

        public MenuItem(ConsoleKeyInfo keyInfo, ConsoleEvent consoleEvent, string title) {
            KeyInfo = keyInfo;
            ConsoleEvent = consoleEvent;
            Title = title;
        }

        public void Print() {
            ConsoleColor background = Console.BackgroundColor;

            if (Selected) {
                Console.BackgroundColor = SelectedColor;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(KeyInfo.Key);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" " + Title);

            Console.BackgroundColor = background;
        }

        public ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo) {
            if (KeyInfo.Equals(keyInfo)) {
                return ConsoleEvent;
            } else {
                return ConsoleEvent.Idle;
            }
        }
    }
}