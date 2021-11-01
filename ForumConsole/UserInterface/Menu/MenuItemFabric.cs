using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public static class MenuItemFabric {
        public static ReactMenuItem CreateEscapeMI(string description) {
            return new ReactMenuItem(ConsoleEvent.Escape, new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), description, -1000);
        }

        public static ReactMenuItem CreateAskMI(string description, ConsoleKey key) {
            return new ReactMenuItem(ConsoleEvent.WriteQuestion, new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static ReactMenuItem CreateToAnswerMI(string description, ConsoleKey key) {
            return new ReactMenuItem(ConsoleEvent.WriteAnswer, new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static ReactMenuItem CreateSaveMi(string description, ConsoleKey key) {
            return new ReactMenuItem(ConsoleEvent.Save, new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static MenuItem CreateTemp() {
            return new ActivatableMenuItem(ConsoleEvent.Idle, ConsoleEvent.Idle, new ConsoleKeyInfo('\0', ConsoleKey.F2, false, false, false), ConsoleKey.F2.ToString(), "тест");
        }
    }
}
