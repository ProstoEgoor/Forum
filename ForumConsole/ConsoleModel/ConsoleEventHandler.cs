using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole.ConsoleModel {
    public class ConsoleEventHandler {
        public ConsoleEvent Type { get; }
        public Action<ConsoleItem, ConsoleEventArgs> HandleMethod { get; set; }

        public ConsoleEventHandler(ConsoleEvent type, Action<ConsoleItem, ConsoleEventArgs> handleMethod) {
            Type = type;
            HandleMethod = handleMethod;
        }
    }
}
