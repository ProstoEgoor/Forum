using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface {
    public abstract class ConsoleItem : IConsoleDisplayable, IConsoleReactive {

        ConsoleItem Prev { get; }
        public ConsoleItem Next { get; set; }

        public IConsoleDisplayable Title { get; set; }

        public Menu Menu { get; } = new Menu();
        public ConsoleEventHandlerController EventHandlers { get; } = new ConsoleEventHandlerController();
        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        protected int WindowTop { get; set; } = 0;

        public ConsoleItem(ConsoleItem prev, IConsoleDisplayable title) {
            Prev = prev;
            Next = this;
            Title = title;
            Menu.RaiseEvent += OnRaiseEvent;

            MenuItem itemEscape;
            if (prev == null) {
                itemEscape = MenuItemFabric.CreateEscapeMI("Выйти");
            } else {
                itemEscape = MenuItemFabric.CreateEscapeMI("Назад");
            }

            Menu.AddMenuItem(itemEscape);

            ConsoleEventHandler escapeEvent = new ConsoleEventHandler(ConsoleEvent.Escape, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                consoleItem.Prev?.Reset();
                consoleItem.Next = consoleItem.Prev;
            });

            EventHandlers.AddHandler(escapeEvent);
        }

        public virtual void Reset() {
            Next = this;
            WindowTop = 0;
            Console.Clear();
        }

        public virtual void Show(int width, int indent = 0, bool briefly = false) {
            WindowTop = Console.WindowTop;
            Console.Clear();
            Menu.Show(width);

            Title.Show(width, indent, false);
            Console.WriteLine();

            Console.WindowTop = WindowTop;
        }

        public virtual bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            return Menu.HandlePressedKey(keyInfo);
        }

        public void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent) {
            EventHandlers.HandleEvent(this, consoleEvent);
            RaiseEvent?.Invoke(obj, consoleEvent);
        }
    }
}
