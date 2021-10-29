using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface {
    public class ShowableConsoleItem<T> : ConsoleItem where T : class {
        public SelectFromList<T> SelectFromList { get; }

        public ShowableConsoleItem(ConsoleItem prev, IConsoleDisplayable title, Func<IReadOnlyList<T>> getContentItems, Action<ConsoleItem, ConsoleEventArgs> selectItem) : base(prev, title) {
            SelectFromList = new SelectFromList<T>(getContentItems);
            SelectFromList.RaiseEvent += OnRaiseEvent;

            ConsoleEventHandler selectEvent = new ConsoleEventHandler(ConsoleEvent.SelectItem, selectItem);
            EventHandlers.AddHandler(selectEvent);
        }

        public override void Show(int width, int indent = 1, bool briefly = true) {
            Console.CursorVisible = false;
            base.Show(width, indent, briefly);

            SelectFromList.Show(width, indent, briefly);

            Console.WindowTop = WindowTop;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) 
                return true;

            if (SelectFromList.HandlePressedKey(keyInfo))
                return true;

            return false;
        }
    }
}
