using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface {
    public class ShowableConsoleItem : ConsoleItem {
        Func<IReadOnlyList<IConsoleDisplayable>> getContentItems;
        public IReadOnlyList<IConsoleDisplayable> ContentItems => getContentItems();

        int position;
        public int Position {
            get => position;
            set {
                if (ContentItems.Count == 0) {
                    position = 0;
                    return;
                }

                if (value > ContentItems.Count - 1) {
                    value = ContentItems.Count - 1;
                } else if (value < 0) {
                    value = 0;
                }

                position = value;
            }
        }

        public bool Selectable { get; set; } = true;
        int SelectedCursorStart { get; set; }
        int SelectedCursorEnd { get; set; }

        public ShowableConsoleItem(ConsoleItem prev, string title, Func<IReadOnlyList<IConsoleDisplayable>> getContentItems, Action<ConsoleItem, ConsoleEventArgs> selectItem) : base(prev, title) {
            this.getContentItems = getContentItems;

            ConsoleEventHandler upEvent = new ConsoleEventHandler(ConsoleEvent.SelectAbove, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                (consoleItem as ShowableConsoleItem).Position--;
            });
            EventHandlers.AddHandler(upEvent);

            ConsoleEventHandler downEvent = new ConsoleEventHandler(ConsoleEvent.SelectBelow, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
                (consoleItem as ShowableConsoleItem).Position++;
            });
            EventHandlers.AddHandler(downEvent);

            ConsoleEventHandler selectEvent = new ConsoleEventHandler(ConsoleEvent.SelectItem, selectItem);
            EventHandlers.AddHandler(selectEvent);
        }

        public override void Show(int width, int indent = 1, bool briefly = true) {
            Console.CursorVisible = false;
            base.Show(width);

            if (ContentItems != null) {
                for (int i = 0; i < ContentItems.Count; i++) {
                    if (Selectable && Position == i) {
                        SelectedCursorStart = Console.CursorTop;
                    }

                    ContentItems[i].Show(width, indent, briefly);

                    if (Selectable && Position == i) {
                        SelectedCursorEnd = Console.CursorTop;
                        Console.BackgroundColor = ConsoleColor.Green;
                        for (int j = SelectedCursorStart; j < SelectedCursorEnd; j++) {
                            Console.CursorLeft = 0;
                            Console.CursorTop = j;
                            Console.Write(' ');
                        }
                        Console.ResetColor();
                        Console.CursorLeft = 0;
                        Console.CursorTop = SelectedCursorEnd;
                    }

                    Console.WriteLine();
                }
            }

            Console.WindowTop = WindowTop;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) {
                return true;
            }

            if (keyInfo.Key == ConsoleKey.UpArrow) {
                OnRaiseEvent(this, new ConsoleEventArgs(ConsoleEvent.SelectAbove));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.DownArrow) {
                OnRaiseEvent(this, new ConsoleEventArgs(ConsoleEvent.SelectBelow));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Enter) {
                OnRaiseEvent(this, new ConsoleEventArgs(ConsoleEvent.SelectItem));
                return true;
            }

            return false;
        }
    }
}
