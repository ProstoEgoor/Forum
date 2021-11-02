using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ExtendableMenuItem<TypeExpandableContent> : ActivatableMenuItem {
        public TypeExpandableContent Content { get; }

        public override bool CursorVisible => (Content as IConsoleDisplayable)?.CursorVisible ?? false;

        public ExtendableMenuItem(TypeExpandableContent content, string activeType, string type, ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) : base(activeType, type, keyInfo, keyTitle, description, order) {
            Content = content;
            if (Content is IConsoleReactive reactiveContent) {
                reactiveContent.RaiseEvent += HandleEvent;
            }
            if(Content is IConsoleDisplayable displayableContent) {
                displayableContent.Foreground = Foreground;
                displayableContent.Background = Background;
            }
        }

        public ExtendableMenuItem(TypeExpandableContent content, string activeType, string type, ConsoleKeyInfo keyInfo, string description, int order = 1) : this(content, activeType, type, keyInfo, keyInfo.Key.ToString(), description, order) { }

        public override void Show((int left, int right) indent) {
            base.Show(indent);

            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            if (Active && Content != null) {
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));

                if (Content is IConsoleDisplayable displayableContent) {
                    displayableContent.Show(indent);
                    Cursor = displayableContent.Cursor;
                } else {
                    int start = -1;
                    string str = Content.ToString();
                    int width = Console.WindowWidth - indent.left - indent.right;
                    Console.ForegroundColor = Foreground;
                    Console.BackgroundColor = Background;
                    while (PrintHelper.TryGetLine(str, width, ref start, out string line)) {
                        Console.Write(new string(' ', indent.left));
                        Console.Write(line);
                        Cursor = (Console.CursorTop, Console.CursorLeft);
                        Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    }
                }
            }
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) {
                return true;
            }

            if (Active && Content is IConsoleReactive reactiveContent) {
                return reactiveContent.HandlePressedKey(keyInfo);
            }

            return false;
        }
    }
}
