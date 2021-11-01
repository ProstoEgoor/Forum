using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.ModelWrapper {
    public interface IConsoleEditableContainer<T> {
        void Add(T item);
        bool Remove(T item);
        bool Replace(T oldItem, T newItem);
    }
}
