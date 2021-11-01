using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.ModelWrapper;
using System.Linq;

namespace ForumConsole.UserInterface {
    public class WriteConsoleItem<TitleType, EditableType> : EntitledConsoleItem<TitleType> {

        public SelectFromList<WriteField> SelectFromList { get; }

        IConsoleEditable<EditableType> EditableItem { get; }
        IConsoleEditableContainer<EditableType> EditableContainer { get; }

        IReadOnlyList<WriteField> WriteFields { get; }
        public WriteConsoleItem(ConsoleItem prev, TitleType title, IConsoleEditable<EditableType> editableItem, IConsoleEditableContainer<EditableType> editableContainer) : base(prev, title) {
            EditableItem = editableItem;
            EditableContainer = editableContainer;
            WriteFields = EditableItem.GetWriteFields;
            SelectFromList = new SelectFromList<WriteField>(() => WriteFields);
            SelectFromList.RaiseEvent += HandleEvent;

            EventHandler.AddHandler(ConsoleEvent.Save, delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                if (WriteFields.All(item => item.IsValide)) {
                    if (EditableItem.Element == null) {
                        EditableType newElement = EditableItem.CreateFromWriteFields(WriteFields);
                        EditableContainer.Add(newElement);
                        OnRaiseEvent(new ConsoleEventArgs(ConsoleEvent.Escape));
                    } else {
                        EditableType newElement = EditableItem.CreateFromWriteFields(WriteFields);
                        EditableContainer.Replace(editableItem.Element, newElement);
                        editableItem.Element = newElement;
                        OnRaiseEvent(new ConsoleEventArgs(ConsoleEvent.Escape));
                    }
                } else {
                    foreach (var item in WriteFields) {
                        item.HighlightError = true;
                    }
                }
            });

        }

        public override void Show(int width, int indent = 0, bool briefly = false) {
            base.Show(width, indent, briefly);

            SelectFromList.Show(width, indent, briefly);

            Console.CursorTop = SelectFromList.SelectedItem.Cursor.Top;
            Console.CursorLeft = SelectFromList.SelectedItem.Cursor.Left;

            if (SelectFromList.SelectedItem.Editable) {
                Console.CursorVisible = true;
            } else {
                Console.CursorVisible = false;
            }
            Console.WindowTop = WindowTop;
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo))
                return true;

            if (SelectFromList.HandlePressedKey(keyInfo))
                return true;

            return false;
        }
    }
}
