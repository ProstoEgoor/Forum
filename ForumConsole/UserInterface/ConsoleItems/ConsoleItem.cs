using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public abstract class ConsoleItem : IConsoleDisplayable {
        public ConsoleItem Prev { get; }
        public ConsoleItem Next { get; protected set; }

        public ConsoleItem() {
            Next = this;
        }
        public ConsoleItem(ConsoleItem prev) {
            Prev = prev;
            Next = this;
        }

        public abstract void Print();
        public abstract ConsoleEvent TakeKey(ConsoleKeyInfo key);
    }
}
