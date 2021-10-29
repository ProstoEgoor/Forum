using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class SelectFromList<T> : IConsoleDisplayable, IConsoleReactive where T : class {

        Func<IReadOnlyList<T>> getContentItems;

        IReadOnlyList<T> contentItems;
        public IReadOnlyList<T> ContentItems {
            get {
                ContentItems = getContentItems();
                return contentItems;
            }
            set {
                contentItems = value;
                if (contentItems != null) {
                    foreach (var item in contentItems) {
                        if (item is IConsoleReactive) {
                            (item as IConsoleReactive).RaiseEvent += OnRaiseEvent;
                        }
                    }
                }
            }

        }

        int position;
        public int Position {
            get => position;
            set {
                if (ContentItems == null || ContentItems.Count == 0) {
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
        public T SelectedItem {
            get => (ContentItems != null && ContentItems.Count > 0) ? ContentItems[Position] : null;
        }

        public bool Selectable { get; set; } = true;
        int SelectedCursorStart { get; set; }
        int SelectedCursorEnd { get; set; }

        public SelectFromList(Func<IReadOnlyList<T>> getContentItems) {
            this.getContentItems = getContentItems;
        }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if ((SelectedItem as IConsoleReactive)?.HandlePressedKey(keyInfo) ?? false) {
                return true;
            }

            if (keyInfo.Key == ConsoleKey.UpArrow) {
                Position--;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.DownArrow) {
                Position++;
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Enter) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(ConsoleEvent.SelectItem));
                return true;
            }

            return false;
        }

        public void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(obj, consoleEvent);
        }

        public void Show(int width, int indent, bool briefly) {
            if (ContentItems != null) {
                for (int i = 0; i < ContentItems.Count; i++) {
                    if (Selectable && Position == i) {
                        SelectedCursorStart = Console.CursorTop;
                    }

                    (ContentItems[i] as IConsoleDisplayable)?.Show(width, indent + 1, briefly);

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

            /*if (Selectable && SelectedCursorEnd - SelectedCursorStart < Console.WindowHeight) {
                if (SelectedCursorEnd > WindowTop + Console.WindowHeight) {
                    WindowTop = SelectedCursorEnd - Console.WindowHeight;
                }

                if (SelectedCursorStart < WindowTop) {
                    WindowTop = SelectedCursorStart;
                }
            }*/
        }
    }
}
