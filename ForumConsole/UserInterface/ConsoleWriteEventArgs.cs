using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ConsoleWriteEventArgs : ConsoleEventArgs {
        public string Tag { get; }
        public bool Valid { get; }
        public object Field { get; }
        public Type FieldType { get; }
        public ConsoleWriteEventArgs(string type, string tag, bool valid, object field, Type fieldType) : base(type) {
            Tag = tag;
            Valid = valid;
            Field = field;
            FieldType = fieldType;
        }
    }
}
