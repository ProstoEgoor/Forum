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

            while (currentItem != null) {
                currentItem.Show(Console.WindowWidth - 1, 1, true);
                currentItem.HandlePressedKey(Console.ReadKey(true));
                currentItem = currentItem.Next;
            }

            Console.Clear();
        }
    }
}
