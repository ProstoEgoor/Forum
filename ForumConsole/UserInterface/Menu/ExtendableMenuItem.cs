using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ExtendableMenuItem<TypeExpandableContent> : ActivatableMenuItem {
        public TypeExpandableContent Content { get; }

        public ExtendableMenuItem(TypeExpandableContent content, ConsoleEvent activeType, ConsoleEvent type, ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) : base(activeType, type, keyInfo, keyTitle, description, order) {
            Content = content;
            if (Content is IConsoleReactive reactiveContent) {
                reactiveContent.RaiseEvent += HandleEvent;
            }
        }

        public ExtendableMenuItem(TypeExpandableContent content, ConsoleEvent activeType, ConsoleEvent type, ConsoleKeyInfo keyInfo, string description, int order = 1) : this(content, activeType, type, keyInfo, keyInfo.Key.ToString(), description, order) { }

        public override void Show(int width, int indent = 0, bool briefly = false) {
            int length = KeyTitle.Length + 1 + Description.Length;
            if (width - Console.CursorLeft < length) {
                Console.WriteLine(new string(' ', width - Console.CursorLeft));
            }

            ConsoleColor backgroung = Console.BackgroundColor;
            if (Active) {
                Console.BackgroundColor = ActiveBackgroundColor;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(KeyTitle);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" " + Description);

            Console.ResetColor();
            Console.BackgroundColor = backgroung;

            if (Active && Content != null) {
                Console.WriteLine(new string(' ', width - Console.CursorLeft));

                if (Content is IConsoleDisplayable displayableContent) {
                    displayableContent.Show(width, indent, false);
                } else {
                    int start = -1;
                    string str = Content.ToString();

                    while (PrintHelper.TryGetLine(str, width - indent - 1, ref start, out string line)) {
                        Console.Write(new string(' ', indent + 1));
                        Console.WriteLine(line);
                    }
                }

                Console.WriteLine();
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
