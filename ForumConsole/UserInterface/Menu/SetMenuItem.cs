using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class SetMenuItem : ActivatableMenuItem {

        List<ActivatableMenuItem> Items { get; } = new List<ActivatableMenuItem>();

        int position = 0;
        public int Position {
            get => position;
            set {
                value = (value + Items.Count) % Items.Count;
                if (position != value) {
                    if (value == 0) {
                        base.Active = false;
                    } else if (position == 0) {
                        base.Active = true;
                    }

                    Items[position].Active = false;
                    position = value;
                    Items[position].Active = true;
                    if (Items[position] is ExtendableMenuItem<WriteField>) {
                        (Items[position] as ExtendableMenuItem<WriteField>).Content.WriteState = true;
                    }
                }
            }
        }

        public int NextPosition {
            get {
                return (Position + 1) % Items.Count;
            }
        }

        public override bool Active {
            set {
                if (Active == false) {
                    base.Active = value;
                    Position = 0;
                }
            }
        }

        public override ConsoleColor Foreground {
            set {
                base.Foreground = value;
                foreach (var item in Items) {
                    item.Foreground = value;
                }
            }
        }
        public override ConsoleColor Background {
            set {
                base.Background = value;
                foreach (var item in Items) {
                    item.Background = value;
                }
                /*Items[0].ActiveBackgroundColor = value;*/
            }
        }

        public override bool CursorVisible => Items[Position].CursorVisible;

        public SetMenuItem(string activeType, string type, ActivatableMenuItem defaultMenuItem, IEnumerable<ActivatableMenuItem> items) : base(activeType, type, defaultMenuItem.KeyInfo, defaultMenuItem.Description) {
            Items.Add(defaultMenuItem);

            foreach (var item in items) {
                item.KeyInfo = defaultMenuItem.KeyInfo;
                item.Type = defaultMenuItem.Type;
                Items.Add(item);
            }

            foreach (var item in Items) {
                if (item is ReactMenuItem reactMenuItem) {
                    reactMenuItem.RaiseEvent += HandleEvent;
                }
            }
        }

        public override void Show((int left, int right) indent) {
            Items[Position].Show(indent);
            Cursor = Items[Position].Cursor;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (KeyInfo == keyInfo) {
                Position = NextPosition;
                return true;
            }

            return (Items[Position] as ReactMenuItem)?.HandlePressedKey(keyInfo) ?? false;
        }
    }
}
