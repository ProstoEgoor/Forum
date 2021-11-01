using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumConsole.UserInterface {
    public class Menu : IConsoleDisplayable, IConsoleReactive {
        List<MenuItem> Items { get; set; } = new List<MenuItem>();

        public Menu() { }
        public Menu(IEnumerable<MenuItem> items) {
            foreach (var item in items) {
                Items.Add(item);
                if (item is ReactMenuItem reactMenuItem) {
                    reactMenuItem.RaiseEvent += HandleEvent;
                }
            }

            Items.Sort((left, right) => left.Order - right.Order);
        }

        public void AddMenuItem(MenuItem item) {
            Items.Add(item);
            if (item is ReactMenuItem reactMenuItem) {
                reactMenuItem.RaiseEvent += HandleEvent;
            }
            Items.Sort((left, right) => left.Order - right.Order);
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
                if ((item as ReactMenuItem)?.HandlePressedKey(keyInfo) ?? false) {
                    return true;
                }
            }

            return false;
        }

        public void HandleEvent(object obj, ConsoleEventArgs consoleEvent) {
            OnRaiseEvent(consoleEvent);
        }

        public void OnRaiseEvent(ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(this, consoleEvent);
        }
    }
}