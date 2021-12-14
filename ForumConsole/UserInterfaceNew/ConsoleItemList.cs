using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterfaceNew {
    public class ConsoleItemList : ConsoleItem {
        List<ConsoleItem> List { get; }
        public override bool Focusable => FocusedItem != null;

        int FocusedItemIndex { get; set; } = 0;
        ConsoleItem FocusedItem {
            get {
                if (FocusedItemIndex >= 0 && FocusedItemIndex < List.Count) {
                    ConsoleItem focusedItem = List[FocusedItemIndex];
                    if (focusedItem.Focusable) {
                        return focusedItem;
                    }
                }

                return null;
            }
        }

        public ConsoleItemList() {
            List = new List<ConsoleItem>();
        }

        public override void Show() {
            foreach (var item in List) {
                item.Show();
            }
        }

        public override bool HandleKeystroke(ConsoleKeyInfo consoleKeyInfo) {
            bool reaction = FocusedItem?.HandleKeystroke(consoleKeyInfo) ?? false;

            if (!reaction) {
                if (FocusedItem != null) {
                    if (consoleKeyInfo.Key == ConsoleKey.UpArrow && FocusedItemIndex > 0) {
                        int prevIndex = FocusedItemIndex;

                        while (--FocusedItemIndex >= 0 && FocusedItem == null) { };

                        if (FocusedItem == null) {
                            FocusedItemIndex = prevIndex;
                            reaction = false;
                        } else {
                            reaction = true;
                        }
                    } else if (consoleKeyInfo.Key == ConsoleKey.DownArrow && FocusedItemIndex < List.Count - 1) {
                        int prevIndex = FocusedItemIndex;

                        while (++FocusedItemIndex < List.Count && FocusedItem == null) { };

                        if (FocusedItem == null) {
                            FocusedItemIndex = prevIndex;
                            reaction = false;
                        } else {
                            reaction = true;
                        }
                    } else if (consoleKeyInfo.Key == ConsoleKey.PageUp) {
                        FocusedItemIndex = List.FindIndex(item => item.Focusable);
                        reaction = FocusedItemIndex != -1;
                    } else if (consoleKeyInfo.Key == ConsoleKey.PageDown) {
                        FocusedItemIndex = List.FindLastIndex(List.Count - 1, item => item.Focusable);
                        reaction = FocusedItemIndex != -1;
                    }
                }
            }

            return reaction;
        }
        public override void OnUpdate() {
            foreach (var item in List) {
                if (item.Adaptable) {
                    item.OnUpdate();
                }
            }
        }

        public virtual void HandleConsoleEvent(object obj, ConsoleEventArgs consoleEventArgs) {
            OnRaiseParentEvent(obj, consoleEventArgs);
        }

    }
}
