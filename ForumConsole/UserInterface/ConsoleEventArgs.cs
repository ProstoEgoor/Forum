using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ConsoleEventArgs : EventArgs {
        public ConsoleEventArgs(ConsoleEvent type) {
            Type = type;
        }

        public ConsoleEvent Type{ get; set; }
    }
}
