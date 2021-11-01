using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class SelectFromList<ContentType> : IConsoleDisplayable, IConsoleReactive where ContentType : class {
        public ConsoleColor SelectedColor { get; set; } = ConsoleColor.Green;
        readonly Func<IReadOnlyList<ContentType>> getContentItems;

        IReadOnlyList<ContentType> contentItems;
        public IReadOnlyList<ContentType> ContentItems {
            get {
                return contentItems;
            }
            private set {
                if (contentItems != value) {
                    contentItems = value;
                    if (contentItems != null) {
                        foreach (var item in contentItems) {
                            if (item is IConsoleReactive) {
                                (item as IConsoleReactive).RaiseEvent += HandleEvent;
                            }
                        }
                    }
                }
            }

        }

        int position;
        public int Position {
            get {
                Position = position;
                return position;
            }
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
        public ContentType SelectedItem {
            get => (ContentItems != null && ContentItems.Count > 0) ? ContentItems[Position] : null;
        }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public bool Selectable { get; set; } = true;
        int SelectedCursorStart { get; set; }
        int SelectedCursorEnd { get; set; }

        public SelectFromList(Func<IReadOnlyList<ContentType>> getContentItems) {
            this.getContentItems = getContentItems;
            UpdateList();
        }

        public void UpdateList() {
            ContentItems = getContentItems();
        }

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if ((SelectedItem as IConsoleReactive)?.HandlePressedKey(keyInfo) ?? false) {
                return true;
            }

            if (keyInfo.Key == ConsoleKey.UpArrow) {
                Position--;
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(ConsoleEvent.SelectAbove));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.DownArrow) {
                Position++;
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(ConsoleEvent.SelectBelow));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Enter) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(ConsoleEvent.SelectItem));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Delete) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs(ConsoleEvent.RemoveItem));
                return true;
            }

            return false;
        }

        public void HandleEvent(object obj, ConsoleEventArgs consoleEvent) {
            OnRaiseEvent(consoleEvent);
        }
        public void OnRaiseEvent(ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(this, consoleEvent);
        }

        public void Show(int width, int indent, bool briefly) {
            if (ContentItems != null) {
                Console.WriteLine();
                for (int i = 0; i < ContentItems.Count; i++) {
                    if (Selectable && Position == i) {
                        SelectedCursorStart = Console.CursorTop;
                    }

                    if (ContentItems[i] is IConsoleDisplayable) {
                        (ContentItems[i] as IConsoleDisplayable)?.Show(width - 1, indent + 1, briefly);
                    } else {
                        int start = -1;
                        string str = ContentItems[i].ToString();

                        while (PrintHelper.TryGetLine(str, width - indent - 1, ref start, out string line)) {
                            Console.Write(new string(' ', indent + 1));
                            Console.WriteLine(line);
                        }
                    }

                    if (Selectable && Position == i) {
                        Console.BackgroundColor = SelectedColor;
                        SelectedCursorEnd = Console.CursorTop;
                        Console.CursorLeft = indent;
                        Console.CursorTop = SelectedCursorStart - 1;
                        Console.WriteLine(new string(' ', width - indent));
                        for (int j = SelectedCursorStart; j < SelectedCursorEnd; j++) {
                            Console.CursorLeft = indent;
                            Console.CursorTop = j;
                            Console.Write(' ');
                            Console.CursorLeft = width - 1;
                            Console.Write(' ');
                        }
                        Console.CursorLeft = indent;
                        Console.CursorTop = SelectedCursorEnd;
                        Console.Write(new string(' ', width - indent));
                        Console.ResetColor();
                    }

                    Console.WriteLine();
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
