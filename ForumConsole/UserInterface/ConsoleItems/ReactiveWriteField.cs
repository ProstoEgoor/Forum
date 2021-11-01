using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ReactiveWriteField<ContentType> : WriteField<ContentType> {
        Func<ContentType> React { get; }

        public override ContentType ParseField {
            get {
                Field.Clear();
                Field.Append(React());
                return base.ParseField;
            }
        }

        public ReactiveWriteField(string title, string field, Func<ContentType> react, Func<string, ContentType> parseField, Predicate<string> validateField) 
            : base(false, title, field, parseField, validateField, new CharType[] { }) {
            React = react;
        }

        public override void Show(int width, int indent, bool briefly) {
            Field.Clear();
            Field.Append(React());
            base.Show(width, indent, briefly);
        }
    }
}
