using System;

namespace ForumConsole.UserInterface {
    public class MenuItem : IConsoleDisplayable, IConsoleReactive {
        public ConsoleKeyInfo KeyInfo { get; }
        public ConsoleEvent Type { get; }
        public string Title { get; }

        public MenuItem(ConsoleKeyInfo keyInfo, ConsoleEvent type, string title) {
            KeyInfo = keyInfo;
            Type = type;
            Title = title;
        }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public void Show(int width, int indent = 0, bool briefly = false) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(KeyInfo.Key);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" " + Title);
        }

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (keyInfo == KeyInfo) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(Type));
                return true;
            }

            return false;
        }

        public void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent) {
            throw new NotImplementedException();
        }
    }
}