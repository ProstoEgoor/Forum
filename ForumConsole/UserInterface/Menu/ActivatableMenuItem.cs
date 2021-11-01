using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ActivatableMenuItem : ReactMenuItem {
        public bool Active { get; set; } = false;
        public ConsoleEvent ActiveType { get; protected set; }
        public ConsoleColor ActiveBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ActivatableMenuItem(ConsoleEvent activeType, ConsoleEvent diactiveType, ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) : base(diactiveType, keyInfo, keyTitle, description, order) {
            ActiveType = activeType;
        }

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

            Console.BackgroundColor = backgroung;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (KeyInfo == keyInfo) {
                Active = !Active;
                OnRaiseEvent(new ConsoleEventArgs(Active ? ActiveType : Type));
                return true;
            }

            return false;
        }
    }
}
