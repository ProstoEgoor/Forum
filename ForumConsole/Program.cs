using System;
using ForumModel;
using ForumConsole.UserInterface;
using ForumConsole.ConsoleModel;

namespace ForumConsole {
    class Program {
        static void Main(string[] args) {
            TagManager tagManager = new TagManager();
            QuestionManagerC questionManager = new QuestionManagerC(tagManager, Mocks.MocksFabric.MockQuestion());

            ConsoleItem currentItem = ConsoleItemFabric.CreateMainItem(questionManager);

            ConsoleKeyInfo keyInfo;
            int windowTop;
            while (currentItem != null) {
                windowTop = Console.WindowTop;
                Console.Clear();
                currentItem.Print(Console.WindowWidth - 1, 1, true);
                Console.WindowTop = windowTop;
                keyInfo = Console.ReadKey(true);
                currentItem.TakeKey(keyInfo);
                currentItem = currentItem.Next;
            }

            Console.Clear();
        }
    }
}
