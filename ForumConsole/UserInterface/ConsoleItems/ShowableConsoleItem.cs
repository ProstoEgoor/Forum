using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole.UserInterface {
    public class ShowableConsoleItem<T> : ConsoleItem {

        public IReadOnlyList<T> ContentItems { get; set; }
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

        int SelectedCursorStart { get; set; }
        int SelectedCursorEnd { get; set; }

        public ShowableConsoleItem(ConsoleItem prev, Menu menu, IReadOnlyList<T> contentItems) : base(prev, menu) {
            ContentItems = contentItems;
        }

        public override void Print(int width, int indent = 1, bool briefly = true) {
            Console.CursorVisible = false;
            base.Print(width);

            if (ContentItems != null) {
                for (int i = 0; i < ContentItems.Count; i++) {
                    if (Position == i) {
                        SelectedCursorStart = Console.CursorTop;
                    }

                    if (ContentItems[i] is IConsolePrintable) {
                        (ContentItems[i] as IConsolePrintable).Print(width, indent, briefly);
                    } else {
                        Console.WriteLine(ContentItems[i].ToString());
                    }

                    if (Position == i && ContentItems[i] is IConsolePrintable) {
                        SelectedCursorEnd = Console.CursorTop;
                        Console.BackgroundColor = ConsoleColor.Green;
                        for (int j = SelectedCursorStart; j < SelectedCursorEnd; j++) {
                            Console.CursorLeft = 0;
                            Console.CursorTop = j;
                            Console.Write(' ');
                        }
                        Console.ResetColor();
                        Console.CursorLeft = 0;
                        Console.CursorTop = SelectedCursorEnd;
                    }

                    Console.WriteLine();
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
