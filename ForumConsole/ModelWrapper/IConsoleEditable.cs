using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole.ModelWrapper {
    public interface IConsoleEditable<T> {
        IReadOnlyList<WriteField> GetWriteFields { get; }
        bool IsEmpty { get; }
        T Element { get; set; }
        T CreateFromWriteFields(IReadOnlyList<WriteField> writeFields);
    }
}
