using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ForumConsole.UserInterface {
    public class SelectFromList<ContentType> : IConsoleDisplayable, IConsoleReactive where ContentType : class {
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
        public int SelectedCursorStart { get; set; }
        public int SelectedCursorEnd { get; set; }
        public ConsoleColor SelectedForeground { get; set; } = ConsoleColor.Black;
        public ConsoleColor SelectedBackground { get; set; } = ConsoleColor.DarkGreen;

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public ConsoleColor Background { get => background; set => background = value; }

        public bool CursorVisible => Selectable && ((ContentItems[Position] as IConsoleDisplayable)?.CursorVisible ?? false);

        public (int top, int left) Cursor { get; set; } = (0, 0);

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
                RaiseEvent?.Invoke(this, new ConsoleEventArgs("SelectAbove"));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.DownArrow) {
                Position++;
                RaiseEvent?.Invoke(this, new ConsoleEventArgs("SelectBelow"));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Enter) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs("SelectItem"));
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Delete) {
                RaiseEvent?.Invoke(this, new ConsoleEventArgs("RemoveItem"));
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

        public void Show((int left, int right) indent, bool briefly) {
            if (ContentItems != null) {
                for (int i = 0; i < ContentItems.Count; i++) {
                    Console.ForegroundColor = Selectable && Position == i ? SelectedForeground : Foreground;
                    Console.BackgroundColor = Selectable && Position == i ? SelectedBackground : Background;
                    if (ContentItems[i] is IConsoleDisplayable displayableItem) {
                        displayableItem.Foreground = Selectable && Position == i ? SelectedForeground : Foreground;
                        displayableItem.Background = Selectable && Position == i ? SelectedBackground : Background;
                    }

                    if (Selectable && Position == i) {
                        SelectedCursorStart = Console.CursorTop;
                    }

                    if (ContentItems[i] is IConsoleDisplayableBriefly displayableBrieflyItem) {
                        displayableBrieflyItem.Show(indent, briefly);
                    } else if (ContentItems[i] is IConsoleDisplayable _displayableItem) {
                        _displayableItem.Show(indent);
                    } else {
                        int start = -1;
                        string str = ContentItems[i].ToString();
                        int width = Console.WindowWidth - indent.left - indent.right;

                        while (PrintHelper.TryGetLine(str, width, ref start, out string line)) {
                            Console.Write(new string(' ', indent.left));
                            Console.Write(line);
                            Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
                        }
                    }

                    Console.CursorVisible = true;
                    if (Selectable && Position == i) {
                        SelectedCursorEnd = Console.CursorTop;
                    }

                    Console.ForegroundColor = Foreground;
                    Console.BackgroundColor = Background;
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }

                Cursor = (ContentItems[position] as IConsoleDisplayable).Cursor;
            }
        }

        public void Show((int left, int right) indent) {
            Show(indent, true);
        }
    }
}
