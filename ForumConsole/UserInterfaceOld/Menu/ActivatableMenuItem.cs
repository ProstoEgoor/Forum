using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ActivatableMenuItem : ReactMenuItem {
        protected bool active = false;
        public virtual bool Active {
            get => active; 
            set {
                if (active != value) {
                    OnRaiseEvent(new ConsoleEventArgs(value ? ActiveType : Type));
                }
                active = value;
            }
        }
        public string ActiveType { get; set; }
        public ConsoleColor ActiveBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ActivatableMenuItem(string activeType, string diactiveType, ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) : base(diactiveType, keyInfo, keyTitle, description, order) {
            ActiveType = activeType;
        }

        public ActivatableMenuItem(string activeType, string diactiveType, ConsoleKeyInfo keyInfo, string description, int order = 1) : this(activeType, diactiveType, keyInfo, keyInfo.Key.ToString(), description, order) { }

        public override void Show((int left, int right) indent) {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = Foreground;

            int length = KeyTitle.Length + 1 + Description.Length;
            if (Console.WindowWidth - indent.right - Console.CursorLeft < length) {
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            if (Active) {
                Console.BackgroundColor = ActiveBackgroundColor;
            }
            Console.ForegroundColor = KeyColor;
            Console.Write(KeyTitle);
            Console.ForegroundColor = Foreground;
            Console.Write(" " + Description);
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (KeyInfo == keyInfo) {
                Active = !Active;
                return true;
            }

            return false;
        }
    }
}
