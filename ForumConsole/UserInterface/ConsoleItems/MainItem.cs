using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;

namespace ForumConsole.UserInterface {
    public class MainItem : ConsoleItem {

        public Menu Menu { get; }
        public QuestionManager QuestionManager { get; }
        public int Position { get; private set; }

        public MainItem(QuestionManager questionManager, Menu menu) : base() {
            QuestionManager = questionManager;
            Menu = menu;
        }

        public override void Print() {
            Menu.Print();

            for (int i = 0; i < QuestionManager.Count; i++) {
                /*if (Position == i) {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }*/

                Console.WriteLine(QuestionManager[i]);

                /*Console.ResetColor();*/
            }
        }

        public override ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo) {
            ConsoleEvent consoleEvent = Menu.TakeKey(keyInfo);
            if (consoleEvent != ConsoleEvent.Idle) {
                if (consoleEvent == ConsoleEvent.Escape) {
                    Next = Prev;
                }
                /*if (consoleEvent == ConsoleEvent.WriteQuestion) {
                    Next = new WriteQuestionItem();
                }
                if (consoleEvent == ConsoleEvent.WriteAnswer) {
                    Next = new WriteAnswerItem();
                }*/
                return consoleEvent;
            }

            switch(keyInfo.Key) {
                case ConsoleKey.Enter:
                    consoleEvent = ConsoleEvent.ShowQuestion;
                    Next = new ShowQuestionItem();
                    break;
                case ConsoleKey.UpArrow:
                    Position = Math.Max(Position - 1, 0);
                    break;
                case ConsoleKey.DownArrow:
                    Position = Math.Min(Position + 1, QuestionManager.Count - 1);
                    break;
            }

            return consoleEvent;
        }
    }
}
