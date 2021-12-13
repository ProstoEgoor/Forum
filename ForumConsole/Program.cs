using System;
using ForumModel;
using ForumConsole.UserInterface;
using ForumConsole.ModelWrapper;
using ForumConsole.DB;

namespace ForumConsole {
    class Program {
        readonly static QuestionManagerWrapper questionManagerWrapper;
        const bool useMocks = false;

        static Program() {
            TagManager tagManager = new TagManager();
            if (useMocks) {
                questionManagerWrapper = new QuestionManagerWrapper(new QuestionManager(tagManager, Mocks.MocksFabric.MockQuestion()));
            } else {
                questionManagerWrapper = new QuestionManagerWrapper(new QuestionManager(tagManager, DBManager.GetQuestions()));
            }
        }

        static void Main(string[] args) {
            //Console.OutputEncoding = System.Text.Encoding.Unicode;

            ConsoleItem currentItem = ConsoleItemFabric.CreateMainItem(questionManagerWrapper);

            while (currentItem != null) {
                currentItem.Show((1, 1));
                currentItem.SetCursor();
                currentItem.HandlePressedKey(Console.ReadKey(true));
                currentItem = currentItem.Next;
            }

            if (!useMocks && !questionManagerWrapper.WasLoad) {
                DBManager.UpdateQuestions(questionManagerWrapper.QuestionManager.Questions);
            }

            Console.Clear();
        }
    }
}
