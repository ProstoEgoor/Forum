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

        public (int Top, int Left) Cursor { get; private set; }
        protected StringBuilder Field { get; } = new StringBuilder();
        protected Predicate<string> ValidateField { get; }

        public bool IsValide {
            get => ValidateField?.Invoke(Field.ToString()) ?? false;
        }

        public List<CharType> AllowedCharTypes { get; } = new List<CharType>();

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
                    Field.Remove(Field.Length - 1, 1);
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

        public virtual void Show(int width, int indent, bool briefly) {
            ConsoleColor background = Console.BackgroundColor;
            ConsoleColor foreground = Console.ForegroundColor;

            if (HighlightError && !IsValide) {
                Console.BackgroundColor = ConsoleColor.Red;
            }

            Console.Write(new string(' ', indent));
            Console.Write(Title);
            Console.Write(": ");
            Cursor = (Console.CursorTop, Console.CursorLeft);

            if (!Editable) {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }

            int start = -1;
            string line;
            string str = Field.ToString();

            if (PrintHelper.TryGetLine(str, width - indent - Title.Length - 2, ref start, out line)) {
                Console.Write(line);
                Cursor = (Console.CursorTop, Console.CursorLeft);
                Console.WriteLine();
            } else {
                Console.WriteLine();
            }

            while (PrintHelper.TryGetLine(str, width - indent, ref start, out line)) {
                Console.Write(new string(' ', indent));
                Console.Write(line);
                Cursor = (Console.CursorTop, Console.CursorLeft);
                Console.WriteLine();
            }

            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
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
