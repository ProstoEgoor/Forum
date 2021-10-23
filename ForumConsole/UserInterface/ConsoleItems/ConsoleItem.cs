using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public abstract class ConsoleItem : IConsoleDisplayable {

        ConsoleItem Prev { get; }
        public ConsoleItem Next { get; protected set; }

        Menu Menu { get; } 
        public delegate ConsoleItem NextMenuDel(ConsoleEvent consoleEvent);
        public NextMenuDel NextMenu { get; set; }

        public ConsoleItem(ConsoleItem prev, Menu menu) {
            Prev = prev;
            Next = this;
            Menu = menu;
        }

        public virtual void Print() {
            Menu.Print();
        }

        public virtual ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo) {
            ConsoleEvent consoleEvent = Menu.TakeKey(keyInfo);
            if (consoleEvent == ConsoleEvent.Escape) {
                Next = Prev;
            } else if (NextMenu != null && consoleEvent != ConsoleEvent.Idle) {
                Next = NextMenu(consoleEvent);
            }
            return consoleEvent;
        }
    }
}
