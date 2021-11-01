using System;
using System.Collections.Generic;
using System.Text;


namespace ForumConsole.UserInterface {
    class ListConsoleItem<TitleType, ContentType> : EntitledConsoleItem<TitleType> where ContentType : class {

        public SelectFromList<ContentType> SelectFromList { get; }

        public ListConsoleItem(ConsoleItem prev, TitleType title, Func<IReadOnlyList<ContentType>> getContentItems, Action<ConsoleItem, ConsoleEventArgs> selectItem, Action<ConsoleItem, ConsoleEventArgs> removeItem) : base(prev, title) {
            SelectFromList = new SelectFromList<ContentType>(getContentItems);
            SelectFromList.RaiseEvent += HandleEvent;

            EventHandler.AddHandler(ConsoleEvent.SelectItem, selectItem);

            EventHandler.AddHandler(ConsoleEvent.RemoveItem, removeItem);

        }

        public override void Show(int width, int indent = 1, bool briefly = true) {
            Console.CursorVisible = false;
            base.Show(width, indent, briefly);

            SelectFromList.UpdateList();
            SelectFromList.Show(width, indent + 1, briefly);

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
