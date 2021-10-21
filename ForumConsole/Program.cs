using System;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole {
    class Program {
        static void Main(string[] args) {
            TagManager tagManager = new TagManager();
            QuestionManager questionManager = new QuestionManager(tagManager);

            ConsoleItem currentItem = new MainItem(questionManager, new Menu(new MenuItem[] {
                MenuItemFabric.CreateEscapeMI("Выйти")
            }));

            ConsoleEvent consoleEvent;
            ConsoleKeyInfo keyInfo;
            while (currentItem != null) {
                Console.Clear();
                currentItem.Print();
                keyInfo = Console.ReadKey(true);
                consoleEvent = currentItem.TakeKey(keyInfo);
                if (consoleEvent != ConsoleEvent.Idle) {
                    currentItem = currentItem.Next;
                }
            }

            Console.Clear();
        }
    }
}
