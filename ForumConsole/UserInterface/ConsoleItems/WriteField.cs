using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ModelWrapper;
using ForumConsole.UserInterface;

namespace ForumConsole.UserInterface {
    public enum CharType {
        letter,
        digit,
        punctuation,
        space,
        lineSeparator
    }
    public abstract class WriteField : IConsoleDisplayable, IConsoleReactive {
        protected string Title { get; }
        public bool Editable { get; }
        public bool HighlightError { get; set; }

        public (int top, int left) Cursor { get; set; } = (0, 0);
        protected StringBuilder Field { get; } = new StringBuilder();
        protected Predicate<string> ValidateField { get; }

        public bool IsValide {
            get => ValidateField?.Invoke(Field.ToString()) ?? false;
        }

        public List<CharType> AllowedCharTypes { get; } = new List<CharType>();

        public ConsoleColor UneditableColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public ConsoleColor Background { get => background; set => background = value; }
        public bool CursorVisible { get => Editable; }

        bool IConsoleDisplayable.CursorVisible => Editable;

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public WriteField(bool editable, string title, string field, Predicate<string> validateField, IEnumerable<CharType> allowedCharTypes) {
            Editable = editable;
            Title = title;
            Field = new StringBuilder(field);
            ValidateField = validateField;
            AllowedCharTypes.AddRange(allowedCharTypes);
        }

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (Editable) {
                if (IsValidChar(keyInfo.KeyChar)) {
                    Field.Append(keyInfo.KeyChar == '\r' ? '\n' : keyInfo.KeyChar);
                    HighlightError = false;
                    return true;
                }

                if (keyInfo.Key == ConsoleKey.Backspace && Field.Length > 0) {
                    int length = (Field.Length > 1 && Field[Field.Length - 1] == '\n' && Field[Field.Length - 2] == '\r') ? 2 : 1;
                    Field.Remove(Field.Length - length, length);
                    HighlightError = false;
                    return true;
                }

            }




            return false;
        }

        private bool IsValidChar(char keyChar) {
            foreach (var charType in AllowedCharTypes) {
                switch (charType) {
                    case CharType.letter:
                        if (char.IsLetter(keyChar))
                            return true;
                        break;
                    case CharType.digit:
                        if (char.IsDigit(keyChar))
                            return true;
                        break;
                    case CharType.punctuation:
                        if (char.IsPunctuation(keyChar))
                            return true;
                        break;
                    case CharType.space:
                        if (keyChar == ' ')
                            return true;
                        break;
                    case CharType.lineSeparator:
                        if (keyChar == '\n' || keyChar == '\r')
                            return true;
                        break;
                }
            }

            return false;
        }

        public virtual void Show((int left, int right) indent) {
            if (HighlightError && !IsValide) {
                Console.BackgroundColor = ErrorColor;
            }

            Console.Write(new string(' ', indent.left));
            Console.Write(Title);
            Console.Write(": ");
            Cursor = (Console.CursorTop, Console.CursorLeft);

            if (!Editable) {
                Console.BackgroundColor = UneditableColor;
            }

            int start = -1;
            string line;
            string str = Field.ToString();
            int width = Console.WindowWidth - indent.left - indent.right;

            if (PrintHelper.TryGetLine(str, width - Title.Length - 2, ref start, out line)) {
                Console.Write(line);
                Cursor = (Console.CursorTop, Console.CursorLeft);
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            } else {
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            while (PrintHelper.TryGetLine(str, width, ref start, out line)) {
                Console.Write(new string(' ', indent.left));
                Console.Write(line);
                Cursor = (Console.CursorTop, Console.CursorLeft);
                Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }

            Console.ResetColor();
        }

        public void OnRaiseEvent(ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(this, consoleEvent);
        }
    }
    public class WriteField<ContentType> : WriteField {

        public virtual ContentType ParseField {
            get {
                if (ParseFieldFunc != null && IsValide) {
                    return ParseFieldFunc(Field.ToString());
                }

                return default;
            }
        }
        Func<string, ContentType> ParseFieldFunc { get; }

        public WriteField(bool editable, string title, string field, Func<string, ContentType> parseField, Predicate<string> validateField, IEnumerable<CharType> allowedCharTypes) : base(editable, title, field, validateField, allowedCharTypes) {
            ParseFieldFunc = parseField;
        }
    }
}
