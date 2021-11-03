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

        public static MenuItem CreateFindMI(ConsoleKey key) {
            return new ActivatableMenuItem("FindOn", "FindOff", new ConsoleKeyInfo('\0', key, false, false, false), "Поиск");
        }

        public static MenuItem CreateFindPropertyMI(ConsoleKey key) {
            return new SetMenuItem("InputFindStart", "InputFindEnd", new ActivatableMenuItem("", "FindChange", new ConsoleKeyInfo('\0', key, false, false, false), "Изменить параметры поиска") { ActiveBackgroundColor = ConsoleColor.DarkCyan }, new ActivatableMenuItem[] {
                new ExtendableMenuItem<WriteField<string>>(new WriteField<string>(true, "FindText", "Текст", "", (field) => field, (field) => true, (int) CharType.All ^ (int) CharType.LineSeparator),
                "FindText", "", new ConsoleKeyInfo('\0', key, false, false, false), "Поиск по тексту"),
                new ExtendableMenuItem<WriteField<string>>(new WriteField<string>(true, "FindTags", "Теги", "", (field) => field, (field) => true, (int) CharType.All ^ (int) CharType.LineSeparator), 
                "FindTags", "", new ConsoleKeyInfo('\0', key, false, false, false), "Поиск по тегам")
            });
        }

        public static MenuItem CreateVoteMI(bool positive) {
            return new ReactMenuItem(positive ? "VotePos" : "VoteNeg", new ConsoleKeyInfo('\0', positive ? ConsoleKey.UpArrow : ConsoleKey.DownArrow, false, false, false), positive ? "\u2191" : "\u2193", string.Format("голосовать {0}", positive ? "за" : "против"));
        }

        public static MenuItem CreateSortMI(ConsoleKey key) {
            return new SetMenuItem("", "", new ActivatableMenuItem("SortAnswersOff", "", new ConsoleKeyInfo('\0', key, false, false, false), "Сортировать") { ActiveBackgroundColor = ConsoleColor.DarkCyan}, new ActivatableMenuItem[] {
                new ActivatableMenuItem("SortAnswerDateByAscending", "", new ConsoleKeyInfo('\0', key, false, false, false), "Сортировать сначала старые"),
                new ActivatableMenuItem("SortAnswerDateByDescending", "", new ConsoleKeyInfo('\0', key, false, false, false), "Сортировать сначала новые"),
                new ActivatableMenuItem("SortAnswerRatingByDescending", "", new ConsoleKeyInfo('\0', key, false, false, false), "Сортировать по рейтингу")
            });
        }

        public static MenuItem CreateShowTagsMI(ConsoleKey key) {
            return new ReactMenuItem("ShowTagsFrequency", new ConsoleKeyInfo('\0', key, false, false, false), "Показать Теги");
        }

        public static MenuItem CreateStateFileMi(ConsoleKey key) {
            return new SetMenuItem("", "", new ActivatableMenuItem("SaveFileState", "", new ConsoleKeyInfo('\0', key, false, false, false), "Режим сохранения") { ActiveBackgroundColor = ConsoleColor.DarkCyan }, new ActivatableMenuItem[] {
                new ActivatableMenuItem("LoadFileState", "", new ConsoleKeyInfo('\0', key, false, false, false), "Режим загрузки") { ActiveBackgroundColor = ConsoleColor.DarkCyan }
            });
        }

        public static MenuItem CreateShowFileMi(ConsoleKey key) {
            return new ReactMenuItem("ShowFileLoader", new ConsoleKeyInfo('\0', key, false, false, false), "Сохраненить/Загрузить");
        }
    }
}
