using System;
using System.Collections.Generic;

namespace ForumConsole.UserInterface {
    public class Menu : IConsolePrintable, IConsoleReactive {
        public List<MenuItem> Items { get; } = new List<MenuItem>();

        public Menu(IEnumerable<MenuItem> items) {
            Items.AddRange(items);
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

        public void Print(int width, int indent = 0, bool briefly = false) {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            foreach (var item in Items) {
                item.Print(width);
                Console.Write(" ");
            }
            Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));

            Console.ResetColor();
        }
    }
}