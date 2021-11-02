using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ConsoleEventArgs : EventArgs {
        public ConsoleEventArgs(string type) {
            Type = type;
        }

        public string Type{ get; set; }
    }
}
