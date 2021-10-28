using System;
using System.Collections.Generic;

namespace ForumConsole.UserInterface {
    public class Menu : IConsoleDisplayable, IConsoleReactive {
        List<MenuItem> Items { get; } = new List<MenuItem>();

        public Menu() { }
        public Menu(IEnumerable<MenuItem> items) {
            foreach (var item in items) {
                Items.Add(item);
                item.RaiseEvent += OnRaiseEvent;
            }
        }

        public void AddMenuItem(MenuItem item) {
            Items.Add(item);
            item.RaiseEvent += OnRaiseEvent;
        }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public void Show(int width, int indent = 0, bool briefly = false) {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            foreach (var item in Items) {
                item.Show(width);
                Console.Write(" ");
            }
            Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));

            Console.ResetColor();
        }

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            foreach (var item in Items) {
                if (item.HandlePressedKey(keyInfo))
                    return true;
            }

            return false;
        }

        public void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(obj, consoleEvent);
        }
    }
}