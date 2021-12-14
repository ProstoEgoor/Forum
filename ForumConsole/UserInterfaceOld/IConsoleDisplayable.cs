using System.Collections.Generic;
using System;

namespace ForumConsole.UserInterface {
    public interface IConsoleDisplayable {
        bool CursorVisible { get; }
        (int top, int left) Cursor { get; set; }
        ConsoleColor Foreground { get; set; }
        ConsoleColor Background { get; set; }
        void Show((int left, int right) indent);
    }
}
