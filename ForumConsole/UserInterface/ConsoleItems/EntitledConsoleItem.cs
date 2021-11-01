using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;
using ForumConsole.ModelWrapper;

namespace ForumConsole.UserInterface {
    public class EntitledConsoleItem<TitleType> : ConsoleItem {
        public TitleType Title { get; }

        public EntitledConsoleItem(ConsoleItem prev, TitleType title) : base(prev) {
            Title = title;
        }


        public override void Show(int width, int indent = 1, bool briefly = true) {
            Console.CursorVisible = false;
            base.Show(width, indent, briefly);

            if (Title is IConsoleDisplayable) {
                (Title as IConsoleDisplayable).Show(width, indent, false);
            } else {
                int start = -1;
                string str = Title.ToString();

                while (PrintHelper.TryGetLine(str, width - indent - 1, ref start, out string line)) {
                    Console.Write(new string(' ', indent + 1));
                    Console.WriteLine(line);
                }
            }

            Console.WriteLine();

            Console.WindowTop = WindowTop;
        }
    }
}
