using System;

namespace ForumConsole.UserInterface {
    interface IConsoleReactive {
        bool HandlePressedKey(ConsoleKeyInfo keyInfo);

        event EventHandler<ConsoleEventArgs> RaiseEvent;
        void OnRaiseEvent(object obj, ConsoleEventArgs consoleEvent);
    }
}
