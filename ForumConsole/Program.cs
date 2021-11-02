using System;
using ForumModel;
using ForumConsole.UserInterface;
using ForumConsole.ModelWrapper;

namespace ForumConsole {
    class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            TagManager tagManager = new TagManager();
            QuestionManager questionManager = new QuestionManager(tagManager, Mocks.MocksFabric.MockQuestion());
            QuestionManagerWrapper questionManagerWrapper = new QuestionManagerWrapper(questionManager);

            ConsoleItem currentItem = ConsoleItemFabric.CreateMainItem(questionManagerWrapper);

            while (currentItem != null) {
                currentItem.Show((1, 1));
                currentItem.SetCursor();
                currentItem.HandlePressedKey(Console.ReadKey(true));
                currentItem = currentItem.Next;
            }

            Console.Clear();
        }
    }
}
