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

            EventHandler.AddHandler(ConsoleEvent.Escape, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Prev?.OnResume();
                consoleItem.Next = consoleItem.Prev;
            });
        }

        public void OnResume() {
            Next = this;
            Console.WindowTop = WindowTop;
        }
        public void OnPause() {
            WindowTop = Console.WindowTop;
            Console.Clear();
        }

        public virtual void Show(int width, int indent = 0, bool briefly = false) {
            WindowTop = Console.WindowTop;
            Console.Clear();
            Menu.Show(width);

            Console.WindowTop = WindowTop;
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
