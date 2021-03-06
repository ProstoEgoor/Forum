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
            : base(false, "", title, field, parseField, validateField, 0) {
            React = react;
        }

        public override void Show((int left, int right) indent) {
            Field.Clear();
            Field.Append(React());
            base.Show(indent);
        }
    }
}
