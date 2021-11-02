﻿using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public class EntitledConsoleItem<TitleType> : ConsoleItem {
        public TitleType Title { get; }
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
        }

        public override void Show((int left, int right) indent) {
            base.Show(indent);

            if (Title is IConsoleDisplayable displayableTitle) {
                displayableTitle.Show((indent.left + 1, indent.right));
            } else {
                int start = -1;
                string str = Title.ToString();
                int width = Console.WindowWidth - indent.left - indent.right - 1;
                Console.ForegroundColor = Foreground;
                Console.BackgroundColor = Background;
                while (PrintHelper.TryGetLine(str, width, ref start, out string line)) {
                    Console.Write(new string(' ', indent.left + 1));
                    Console.Write(line);
                    Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
                }
            }

            Console.WriteLine(new string(' ', Console.WindowWidth));

            Console.WindowTop = WindowTop;
            Console.ResetColor();

            if (CursorVisible && !base.CursorVisible && ((Title as IConsoleDisplayable)?.CursorVisible ?? false)) {
                Cursor = (Title as IConsoleDisplayable).Cursor;
            }
        }
    }
}
