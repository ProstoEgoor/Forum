using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ModelWrapper;
using ForumConsole.UserInterface;

namespace ForumConsole.UserInterface {
    public enum CharType {
        Letter = 1,
        Digit = 2,
        Space = 4,
        Punctuation = 8,
        Symbol = 16,
        LineSeparator = 32,
        All = 63
    }
    public abstract class WriteField : IConsoleDisplayable, IConsoleReactive {
        public string EventTag { get; }
        protected string Title { get; }
        public bool Editable { get; } = true;
        public bool WriteState { get; set; } = true;
        public bool HighlightError { get; set; }

        public (int top, int left) Cursor { get; set; } = (0, 0);
        protected StringBuilder Field { get; } = new StringBuilder();
        protected Predicate<string> ValidateField { get; }

        public bool IsValid {
            get => ValidateField?.Invoke(Field.ToString()) ?? false;
        }

        public int AllowedCharTypes { get; } = (int) CharType.All;

        public ConsoleColor UneditableColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

        ConsoleColor foreground = ConsoleColor.Gray;
        ConsoleColor background = ConsoleColor.Black;
        public ConsoleColor Foreground { get => foreground; set => foreground = value; }
        public ConsoleColor Background { get => background; set => background = value; }
        public bool CursorVisible { get => Editable; }

        bool IConsoleDisplayable.CursorVisible => Editable;

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public WriteField(bool editable, string eventTag, string title, string field, Predicate<string> validateField, int allowedCharTypes) {
            Editable = editable;
            EventTag = eventTag;
            Title = title;
            Field = new StringBuilder(field);
            ValidateField = validateField;
            AllowedCharTypes = allowedCharTypes;
        }

        public virtual bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (Editable) {
                if (IsValidChar(keyInfo.KeyChar)) {
                    Field.Append(keyInfo.KeyChar == '\r' ? '\n' : keyInfo.KeyChar);
                    HighlightError = false;
                    WriteState = true;
                    return true;
                }

                if (keyInfo.Key == ConsoleKey.Backspace && Field.Length > 0) {
                    int length = (Field.Length > 1 && Field[Field.Length - 1] == '\n' && Field[Field.Length - 2] == '\r') ? 2 : 1;
                    Field.Remove(Field.Length - length, length);
                    HighlightError = false;
                    WriteState = true;
                    return true;
                }

            }
            return false;
        }

        private bool IsValidChar(char keyChar) {
            if (char.IsLetter(keyChar) && ((AllowedCharTypes & (int) CharType.Letter) != 0)) return true;

            if (char.IsDigit(keyChar) && ((AllowedCharTypes & (int)CharType.Digit) != 0)) return true;

            if (keyChar == ' ' && ((AllowedCharTypes & (int)CharType.Space) != 0)) return true;

            if (char.IsPunctuation(keyChar) && ((AllowedCharTypes & (int)CharType.Punctuation) != 0)) return true;

            if (char.IsSymbol(keyChar) && ((AllowedCharTypes & (int)CharType.Symbol) != 0)) return true;

            if ((keyChar == '\n' || keyChar == '\r') && ((AllowedCharTypes & (int)CharType.LineSeparator) != 0)) return true;

            return false;
        }

        public virtual void Show((int left, int right) indent) {
            if (HighlightError && !IsValid) {
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
                if (ParseFieldFunc != null && IsValid) {
                    return ParseFieldFunc(Field.ToString());
                }

                return default;
            }
        }
        Func<string, ContentType> ParseFieldFunc { get; }

        public WriteField(bool editable, string eventTag, string title, string field, Func<string, ContentType> parseField, Predicate<string> validateField, int allowedCharTypes) 
            : base(editable, eventTag, title, field, validateField, allowedCharTypes) {
            ParseFieldFunc = parseField;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) {
                return true;
            }

            if (keyInfo.Key == ConsoleKey.Enter && WriteState) {
                if (!IsValid) {
                    HighlightError = true;
                }
                OnRaiseEvent(new ConsoleWriteEventArgs("WriteFieldEnd", EventTag, IsValid, ParseField, typeof(ContentType)));
                WriteState = false;
                return true;
            }

            return false;
        }
    }
}
