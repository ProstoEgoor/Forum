using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public static class MenuItemFabric {
        public static ReactMenuItem CreateEscapeMI(string description) {
            return new ReactMenuItem("Escape", new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), description, -1000);
        }

        public static ReactMenuItem CreateAskMI(string description, ConsoleKey key) {
            return new ReactMenuItem("WriteQuestion", new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static ReactMenuItem CreateToAnswerMI(string description, ConsoleKey key) {
            return new ReactMenuItem("WriteAnswer", new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static ReactMenuItem CreateSaveMi(string description, ConsoleKey key) {
            return new ReactMenuItem("Save", new ConsoleKeyInfo('\0', key, false, false, false), description);
        }

        public static MenuItem CreateSet() {
            return new SetMenuItem("", "FindOff", new ActivatableMenuItem("", "FindChange", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Поиск"), new ActivatableMenuItem[] {
                new ExtendableMenuItem<string>("текст", "FindText", "", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Поиск по тексту"),
                new ExtendableMenuItem<string>("теги", "FindTags", "", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Поиск по тегам")
            });
        }
    }
}
