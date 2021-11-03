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

        public override bool CursorVisible => base.CursorVisible || SelectFromList.CursorVisible;

        IReadOnlyList<WriteField> WriteFields { get; }
        public WriteConsoleItem(ConsoleItem prev, TitleType title, IConsoleEditable<EditableType> editableItem, IConsoleEditableContainer<EditableType> editableContainer) : base(prev, title) {
            EditableItem = editableItem;
            EditableContainer = editableContainer;
            WriteFields = EditableItem.GetWriteFields;
            SelectFromList = new SelectFromList<WriteField>(() => WriteFields);
            SelectFromList.RaiseEvent += HandleEvent;

            EventHandler.AddHandler("Save", delegate (ConsoleItem consoleItem, ConsoleEventArgs consoleEventArgs) {
                if (WriteFields.All(item => item.IsValid)) {
                    if (editableItem.IsEmpty) {
                        EditableType newElement = EditableItem.CreateFromWriteFields(WriteFields);
                        EditableContainer.Add(newElement);
                        HandleEvent(this, new ConsoleEventArgs("Escape"));
                    } else {
                        EditableType newElement = EditableItem.CreateFromWriteFields(WriteFields);
                        EditableContainer.Replace(editableItem.Element, newElement);
                        editableItem.Element = newElement;
                        HandleEvent(this, new ConsoleEventArgs("Escape"));
                    }
                } else {
                    foreach (var item in WriteFields) {
                        item.HighlightError = true;
                    }
                }
            });

        }

        public override void Show((int left, int right) indent) {
            base.Show(indent);

            SelectFromList.Show((indent.left + 1, indent.right));

            if (SelectFromList.Selectable && SelectFromList.SelectedCursorEnd - SelectFromList.SelectedCursorStart < Console.WindowHeight) {
                if (SelectFromList.SelectedCursorEnd > WindowTop + Console.WindowHeight) {
                    WindowTop = SelectFromList.SelectedCursorEnd - Console.WindowHeight;
                }

                if (SelectFromList.SelectedCursorStart < WindowTop) {
                    WindowTop = SelectFromList.SelectedCursorStart;
                }
            }

            Console.WindowTop = WindowTop;

            if (CursorVisible && !base.CursorVisible) {
                Cursor = SelectFromList.Cursor;
            }
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
