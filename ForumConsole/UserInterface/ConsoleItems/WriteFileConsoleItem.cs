using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.FIles;
using System.IO;

namespace ForumConsole.UserInterface {
    public class WriteFileConsoleItem : EntitledConsoleItem<string> {
        public IFileEditable EditableItem { get; }
        public bool Save { get; set; } = true;
        public WriteField<string> PathField { get; }

        public override ConsoleColor Foreground {
            set {
                base.Foreground = value;
                PathField.Foreground = value;
            }
        }
        public override ConsoleColor Background {
            set {
                base.Background = value;
                PathField.Background = value;
            }
        }

        public override bool CursorVisible => base.CursorVisible || PathField.CursorVisible;

        public WriteFileConsoleItem(ConsoleItem prev, string title, IFileEditable editableItem) : base(prev, title) {
            EditableItem = editableItem;
            PathField = new WriteField<string>(true, "PathField", "Относительный путь файла", "", (field) => field, (field) => (Save || File.Exists(field)) && field.Trim().Length > 0, (int)CharType.All ^ (int)CharType.LineSeparator);
            PathField.RaiseEvent += HandleEvent;

            UpdateTitle(Save ? "Сохранение:" : "Загрузка:");

            Menu.AddMenuItem(MenuItemFabric.CreateStateFileMi(ConsoleKey.F1));

            EventHandler.AddHandler("SaveFileState", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                Save = true;
                UpdateTitle("Сохранение:");
                (consoleItem as WriteFileConsoleItem).PathField.WriteState = false;
            });

            EventHandler.AddHandler("LoadFileState", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                Save = false;
                UpdateTitle("Загрузка:");
                (consoleItem as WriteFileConsoleItem).PathField.WriteState = false;
            });

            EventHandler.AddHandler("WriteFieldEnd", delegate (ConsoleItem consoleItem, ConsoleEventArgs e) {
                if (e is ConsoleWriteEventArgs writeEvent) {
                    if (writeEvent.Tag == "PathField" && writeEvent.FieldType.Equals(typeof(string)) && writeEvent.Valid) {
                        if (Save) {
                            if (EditableItem.Save((writeEvent.Field as string) ?? "", out string error)) {
                                UpdateTitle("Сохранение: успешно");
                            } else {
                                UpdateTitle($"Ошибка сохранения: {error}");
                            }
                        } else {
                            if (EditableItem.Load((writeEvent.Field as string) ?? "", out string error)) {
                                UpdateTitle("Загрузка: успешно");
                            } else {
                                UpdateTitle($"Ошибка сохранения: {error}");
                            }
                        }
                    }
                }
            });
        }

        public void UpdateTitle(string description) {
            Title = $"Текущая директория {Environment.CurrentDirectory}, используется разделитель {Path.DirectorySeparatorChar} \r\n{description}";
        }

        public override void Show((int left, int right) indent) {
            base.Show(indent);
            PathField.Show(indent);

            if (CursorVisible) {
                if (base.CursorVisible) {
                    Cursor = base.Cursor;
                } else {
                    Cursor = PathField.Cursor;
                }
            }
        }

        public override bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (base.HandlePressedKey(keyInfo)) {
                return true;
            }

            return PathField.HandlePressedKey(keyInfo);
        }
    }
}
