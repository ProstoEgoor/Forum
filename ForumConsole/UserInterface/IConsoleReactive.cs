using System;

namespace ForumConsole.UserInterface {
    interface IConsoleReactive {
        ConsoleEvent TakeKey(ConsoleKeyInfo keyInfo);
    }
}
