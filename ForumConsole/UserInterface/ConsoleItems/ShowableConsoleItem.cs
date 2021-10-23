using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole.UserInterface {
    public class ShowableConsoleItem<T> : ConsoleItem {

        public IReadOnlyList<T> ContentItems { get; }
        public int Position { get; private set; }
        int NextPosition {
            get {
                if (ContentItems.Count == 0) {
                    return 0;
                }

                return (Position + 1) % ContentItems.Count;
            }
        }

        int PrevPosition {
            get {
                if (ContentItems.Count == 0) {
                    return 0;
                }

                return (Position + ContentItems.Count - 1) % ContentItems.Count;
            }
        }

        public bool Selectable { get; set; }
        public delegate ConsoleItem NextContentDel(int position);
        public NextContentDel NextContent { get; set; }

        public ShowableConsoleItem(ConsoleItem prev, Menu menu, IReadOnlyList<T> contentItems) : base(prev, menu) {
            ContentItems = contentItems;
        }

        public override void Print() {
            base.Print();
            for (int i = 0; i < ContentItems.Count; i++) {
                if (Position == i) {
                    Console.BackgroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(ContentItems[i].ToString());

                if (Position == i) {
                    Console.ResetColor();
                }
            }
        }

        public override ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo) {
            ConsoleEvent consoleEvent = base.TakeKey(keyInfo);

            if (consoleEvent == ConsoleEvent.Idle) {
                if (Selectable && NextContent != null && keyInfo.Key == ConsoleKey.Enter) {
                    consoleEvent = ConsoleEvent.ShowSelectedContent;
                    Next = NextContent(Position);
                } else if (keyInfo.Key == ConsoleKey.UpArrow) {
                    Position = PrevPosition;
                } else if (keyInfo.Key == ConsoleKey.DownArrow) {
                    Position = NextPosition;
                }
            }

            return consoleEvent;
        }
    }
}
