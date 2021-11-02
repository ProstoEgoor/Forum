using System;
using System.Collections.Generic;
using System.Text;


namespace ForumConsole.UserInterface {
    class ListConsoleItem<TitleType, ContentType> : EntitledConsoleItem<TitleType> where ContentType : class {

        public bool Briefly { get; set; } = true;
        public SelectFromList<ContentType> SelectFromList { get; }
        public override ConsoleColor Foreground {
            set {
                base.Foreground = value;
                SelectFromList.Foreground = Foreground;
            }
        }
        public override ConsoleColor Background {
            set {
                base.Background = value;
                SelectFromList.Background = Background;
            }
        }

        public override bool CursorVisible => base.CursorVisible || SelectFromList.CursorVisible;

        public ListConsoleItem(ConsoleItem prev, TitleType title, Func<IReadOnlyList<ContentType>> getContentItems, Action<ConsoleItem, ConsoleEventArgs> selectItem, Action<ConsoleItem, ConsoleEventArgs> removeItem) : base(prev, title) {
            SelectFromList = new SelectFromList<ContentType>(getContentItems);
            SelectFromList.RaiseEvent += HandleEvent;
            SelectFromList.Foreground = Foreground;
            SelectFromList.Background = Background;

            EventHandler.AddHandler("SelectItem", selectItem);

            EventHandler.AddHandler("RemoveItem", removeItem);

            SelectFromList.UpdateList();
        }

        public override void Show((int left, int right) indent) {
            base.Show(indent);

            SelectFromList.Show((indent.left + 1, indent.right), Briefly);

            if (SelectFromList.Selectable && SelectFromList.SelectedCursorEnd - SelectFromList.SelectedCursorStart < Console.WindowHeight) {
                if (SelectFromList.SelectedCursorEnd > WindowTop + Console.WindowHeight) {
                    WindowTop = SelectFromList.SelectedCursorEnd - Console.WindowHeight;
                }

                if (SelectFromList.SelectedCursorStart < WindowTop) {
                    WindowTop = SelectFromList.SelectedCursorStart;
                }
            }

            Console.WindowTop = WindowTop;

            if (CursorVisible && !base.CursorVisible) {
                Cursor = SelectFromList.Cursor;
            }
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo))
                return true;

            if (SelectFromList.HandlePressedKey(keyInfo))
                return true;

            return false;
        }

        public override void OnResume() {
            base.OnResume();
            SelectFromList.UpdateList();
        }
    }
}
