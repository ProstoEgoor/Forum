using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterfaceNew {
    public class ConsoleEventHandler {
        Dictionary<string, Action<ConsoleView, ConsoleItem, ConsoleEventArgs>> HandlerDictionary { get; }

        public ConsoleEventHandler() {
            HandlerDictionary = new Dictionary<string, Action<ConsoleView, ConsoleItem, ConsoleEventArgs>>();
        }

        public void AddHandler(string type, Action<ConsoleView, ConsoleItem, ConsoleEventArgs> handler) {
            if (HandlerDictionary.ContainsKey(type)) {
                HandlerDictionary[type] += handler;
            } else {
                HandlerDictionary[type] = handler;
            }

        }

        public void HandleConsoleEvent(ConsoleView view, ConsoleItem sender, ConsoleEventArgs consoleEventArgs) {
            if (HandlerDictionary.ContainsKey(consoleEventArgs.Type)) {
                HandlerDictionary[consoleEventArgs.Type].Invoke(view, sender, consoleEventArgs);
            }
        }
    }
}
