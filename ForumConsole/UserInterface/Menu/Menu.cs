using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumConsole.UserInterface {
    public class Menu : IConsoleDisplayable, IConsoleReactive {
        ConsoleColor foreground = ConsoleColor.White;
        ConsoleColor background = ConsoleColor.DarkCyan;
        public ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public ConsoleColor Background { get => background; set => background = value; }
        List<MenuItem> Items { get; set; } = new List<MenuItem>();

        public bool CursorVisible => Items.Any(item => item.CursorVisible);

        public (int top, int left) Cursor { get; set; } = (0, 0);

        public Menu() { }
        public Menu(IEnumerable<MenuItem> items) {
            foreach (var item in items) {
                Items.Add(item);
                if (item is ReactMenuItem reactMenuItem) {
                    reactMenuItem.RaiseEvent += HandleEvent;
                }
                if (item is IConsoleDisplayable displayableItem) {
                    displayableItem.Foreground = Foreground;
                    displayableItem.Background = Background;
                }
            }

            Items.Sort((left, right) => left.Order - right.Order);
        }

        public void AddMenuItem(MenuItem item) {
            Items.Add(item);
            if (item is ReactMenuItem reactMenuItem) {
                reactMenuItem.RaiseEvent += HandleEvent;
            }
            if (item is IConsoleDisplayable displayableItem) {
                displayableItem.Foreground = Foreground;
                displayableItem.Background = Background;
            }
            Items.Sort((left, right) => left.Order - right.Order);
        }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public void Show((int left, int right) indent) {
            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
            Console.Write(new string(' ', indent.left));

            foreach (var item in Items) {
                item.Show(indent);
                Console.ForegroundColor = Foreground;
                Console.BackgroundColor = Background;
                Cursor = item.Cursor;
                Console.Write(" ");
            }
            if (Console.CursorLeft > 1) {
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            Console.CursorLeft = 0;

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