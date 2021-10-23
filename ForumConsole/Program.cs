using System;
using ForumModel;
using ForumConsole.UserInterface;

namespace ForumConsole {
    class Program {
        static void Main(string[] args) {
            TagManager tagManager = new TagManager();
            QuestionManager questionManager = new QuestionManager(tagManager);

            ConsoleItem currentItem = ConsoleItemFabric.CreateMainItem(questionManager);

            ConsoleEvent consoleEvent;
            ConsoleKeyInfo keyInfo;
            while (currentItem != null) {
                Console.Clear();
                currentItem.Print();
                keyInfo = Console.ReadKey(true);
                currentItem.TakeKey(keyInfo);
                currentItem = currentItem.Next;
            }

            Console.Clear();
        }
    }
}
