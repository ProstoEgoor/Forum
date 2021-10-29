using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ConsoleModel;

namespace ForumConsole.UserInterface.ConsoleItems {
    public enum CharType {
        letter,
        digit,
        punctuation,
        whiteSpace,
        lineSeparator
    }
    public class WriteField<T> : IConsoleDisplayable, IConsoleReactive {
        string Title { get; }
        public bool Editable { get; set; } = true;
        StringBuilder Field { get; } = new StringBuilder();
        public T ParseField { 
            get {
                if (ParseFieldFunc != null && (ValidateField?.Invoke(Field.ToString()) ?? false)) {
                    return ParseFieldFunc(Field.ToString());
                }

                return default;
            }
        }
        Func<string, T> ParseFieldFunc { get; }
        public Predicate<string> ValidateField { get; }

        public List<CharType> AllowedCharTypes { get; } = new List<CharType>();

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public WriteField(Func<string, T> parseField, Predicate<string> validateField, IEnumerable<CharType> allowedCharTypes) {
            ParseFieldFunc = parseField;
            ValidateField = validateField;
            AllowedCharTypes.AddRange(allowedCharTypes);
        }

        public WriteField(string field, Func<string, T> parseField, Predicate<string> validateField, IEnumerable<CharType> allowedCharTypes)
            : this(parseField, validateField, allowedCharTypes) {
            Field = new StringBuilder(field);
        }

        public bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (Editable) {
                if (IsValidChar(keyInfo.KeyChar)) {
                    Field.Append(keyInfo.KeyChar);
                    return true;
                }

                if (keyInfo.Key == ConsoleKey.Backspace && Field.Length > 0) {
                    Field.Remove(Field.Length - 1, 1);
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
                    case CharType.whiteSpace:
                        if (char.IsWhiteSpace(keyChar))
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

        public void Show(int width, int indent, bool briefly) {
            Console.Write(new string(' ', indent));
            Console.Write(Title);
            Console.Write(": ");
            Console.WriteLine(Field);
        }

        public void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent) {
            throw new NotImplementedException();
        }
    }
}
