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

        public static MenuItem CreateFindMI() {
            return new ActivatableMenuItem("FindOn", "FindOff", new ConsoleKeyInfo('\0', ConsoleKey.F2, false, false, false), "Поиск");
        }

        public static MenuItem CreateSet() {
            return new SetMenuItem("InputFindStart", "InputFindEnd", new ActivatableMenuItem("", "FindChange", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Параметры поиска"), new ActivatableMenuItem[] {
                new ExtendableMenuItem<WriteField<string>>(new WriteField<string>(true, "FindText", "Текст", "", (field) => field, (field) => true, (int) CharType.All ^ (int) CharType.LineSeparator),
                "FindText", "", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Поиск по тексту"),
                new ExtendableMenuItem<WriteField<string>>(new WriteField<string>(true, "FindTags", "Теги", "", (field) => field, (field) => true, (int) CharType.All ^ (int) CharType.LineSeparator), 
                "FindTags", "", new ConsoleKeyInfo('\0', ConsoleKey.F3, false, false, false), "Поиск по тегам")
            });
        }
    }
}
