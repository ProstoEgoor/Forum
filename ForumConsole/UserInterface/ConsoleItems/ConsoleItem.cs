using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public abstract class ConsoleItem : IConsoleDisplayable, IConsoleReactive {

        ConsoleItem Prev { get; }
        public ConsoleItem Next { get; set; }
        public Menu Menu { get; } = new Menu();
        public ConsoleEventHandler EventHandler { get; } = new ConsoleEventHandler();
        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        protected int WindowTop { get; set; } = 0;

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public virtual ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public virtual ConsoleColor Background { get => background; set => background = value; }

        public virtual bool CursorVisible => Menu.CursorVisible;

        public (int top, int left) Cursor { get; set; } = (0, 0);

        public ConsoleItem(ConsoleItem prev) {
            Prev = prev;
            Next = this;
            Menu.RaiseEvent += HandleEvent;

            MenuItem itemEscape;
            if (prev == null) {
                itemEscape = MenuItemFabric.CreateEscapeMI("Выйти");
            } else {
                itemEscape = MenuItemFabric.CreateEscapeMI("Назад");
            }

            Menu.AddMenuItem(itemEscape);

            EventHandler.AddHandler("Escape", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Prev?.OnResume();
                consoleItem.Next = consoleItem.Prev;
            });
        }

        public virtual void OnResume() {
            Next = this;
            Console.WindowTop = WindowTop;
        }
        public virtual void OnPause() {
            WindowTop = Console.WindowTop;
            Console.Clear();
        }

        public virtual void Show((int left, int right) indent) {
            WindowTop = Console.WindowTop;
            Console.Clear();
            Menu.Show(indent);
            Cursor = Menu.Cursor;

            Console.WindowTop = WindowTop;
        }

        public void SetCursor() {
            if (CursorVisible) {
                Console.CursorTop = Cursor.top;
                Console.CursorLeft = Cursor.left;
                Console.CursorVisible = true;
            } else {
                Console.CursorVisible = false;
            }
        }

        public virtual bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            return Menu.HandlePressedKey(keyInfo);
        }

        public void HandleEvent(object obj, ConsoleEventArgs consoleEvent) {
            EventHandler.HandleEvent(this, consoleEvent);
            OnRaiseEvent(consoleEvent);
        }

        public void OnRaiseEvent(ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(this, consoleEvent);
        }
    }
}
