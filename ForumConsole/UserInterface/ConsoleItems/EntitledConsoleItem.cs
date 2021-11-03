using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public class EntitledConsoleItem<TitleType> : ConsoleItem {
        public TitleType Title { get; set; }
        public override ConsoleColor Foreground {
            set {
                base.Foreground = value;
                if (Title is IConsoleDisplayable displayableTitle) {
                    displayableTitle.Foreground = Foreground;
                }
            }
        }
        public override ConsoleColor Background {
            set {
                base.Background = value;
                if (Title is IConsoleDisplayable displayableTitle) {
                    displayableTitle.Background = Background;
                }
            }
        }

        public override bool CursorVisible => base.CursorVisible || ((Title as IConsoleDisplayable)?.CursorVisible ?? false);

        public EntitledConsoleItem(ConsoleItem prev, TitleType title) : base(prev) {
            Title = title;
            if (Title is IConsoleDisplayable displayableTitle) {
                displayableTitle.Foreground = Foreground;
                displayableTitle.Background = Background;
            }

            if (Title is IConsoleReactive reactiveTitle) {
                reactiveTitle.RaiseEvent += HandleEvent;
            }
        }

        public override void Show((int left, int right) indent) {
            base.Show(indent);

            if (Title != null) {
                Console.ForegroundColor = Foreground;
                Console.BackgroundColor = Background;
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));

                if (Title is IConsoleDisplayable displayableTitle) {
                    displayableTitle.Show((indent.left, indent.right));
                } else {
                    int start = -1;
                    string str = Title.ToString();
                    int width = Console.WindowWidth - indent.left - indent.right;
                    while (PrintHelper.TryGetLine(str, width, ref start, out string line)) {
                        Console.Write(new string(' ', indent.left));
                        Console.Write(line);
                        Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    }
                }
            }

            Console.WriteLine(new string(' ', Console.WindowWidth));

            Console.WindowTop = WindowTop;
            Console.ResetColor();

            if (CursorVisible && !base.CursorVisible && ((Title as IConsoleDisplayable)?.CursorVisible ?? false)) {
                Cursor = (Title as IConsoleDisplayable).Cursor;
            }
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) {
                return true;
            }

            if (Title is IConsoleReactive reactiveTitle) {
                return reactiveTitle.HandlePressedKey(keyInfo);
            }

            return false;
        }
    }
}
