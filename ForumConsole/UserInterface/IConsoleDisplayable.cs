using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public enum ConsoleEvent {
        Escape,
        ShowSelectedContent,
        ShowQuestion,
        WriteQuestion,
        WriteAnswer,
        Idle
    }
    public interface IConsoleDisplayable {
        public void Print();
        ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo);
    }
}
