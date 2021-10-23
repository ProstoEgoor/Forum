using System;
using System.Collections.Generic;

namespace ForumConsole.UserInterface {
    public class Menu : IConsoleDisplayable {
        public List<MenuItem> Items { get; } = new List<MenuItem>();

        public Menu(IEnumerable<MenuItem> items) {
            Items.AddRange(items);
        }

        public void Print() {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            foreach (var item in Items) {
                item.Print();
                Console.Write(" ");
            }
            Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));

            Console.ResetColor();
        }

        public ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo) {
            ConsoleEvent consoleEvent;
            foreach (var item in Items) {
                consoleEvent = item.TakeKey(keyInfo);
                if (consoleEvent != ConsoleEvent.Idle) {
                    return consoleEvent;
                }
            }
            return ConsoleEvent.Idle;
        }
    }
}