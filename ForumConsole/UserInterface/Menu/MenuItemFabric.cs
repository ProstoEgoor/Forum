using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public static class MenuItemFabric {
        public static MenuItem CreateEscapeMI(string title) {
            return new MenuItem(new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), ConsoleEvent.Escape, title); 
        }

        public static MenuItem CreateAskMI(string title, ConsoleKey key) {
            return new MenuItem(new ConsoleKeyInfo('\0', key, false, false, false), ConsoleEvent.WriteQuestion, title);
        }

        public static MenuItem CreateToAnswerMI(string title, ConsoleKey key) {
            return new MenuItem(new ConsoleKeyInfo('\0', key, false, false, false), ConsoleEvent.WriteAnswer, title);
        }
    }
}
