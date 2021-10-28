using System;
using ForumModel;
using ForumConsole.UserInterface;
using ForumConsole.ConsoleModel;

namespace ForumConsole {
    class Program {
        static void Main(string[] args) {
            TagManager tagManager = new TagManager();
            QuestionManager questionManager = new QuestionManager(tagManager, Mocks.MocksFabric.MockQuestion());
            QuestionManagerWrapper questionManagerWrapper = new QuestionManagerWrapper(questionManager);

            ConsoleItem currentItem = ConsoleItemFabric.CreateMainItem(questionManagerWrapper);

            ConsoleKeyInfo keyInfo;
            while (currentItem != null) {
                currentItem.Show(Console.WindowWidth - 1, 1, true);
                keyInfo = Console.ReadKey(true);
                currentItem.HandlePressedKey(keyInfo);
                currentItem = currentItem.Next;
            }

            Console.Clear();
        }
    }
}
