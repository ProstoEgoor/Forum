using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole {
    public class ConsoleEventHandler {

        Dictionary<string, Action<ConsoleItem, ConsoleEventArgs>> HandlerDictionary { get; } = new Dictionary<string, Action<ConsoleItem, ConsoleEventArgs>>();

        public void AddHandler(string type, Action<ConsoleItem, ConsoleEventArgs> handler) {
            if (HandlerDictionary.ContainsKey(type)) {
                HandlerDictionary[type] += handler;
            } else {
                HandlerDictionary[type] = handler;
            }

        }

        public void HandleEvent(ConsoleItem consoleItem, ConsoleEventArgs consoleEvent) {
            if (HandlerDictionary.ContainsKey(consoleEvent.Type)) {
                HandlerDictionary[consoleEvent.Type]?.Invoke(consoleItem, consoleEvent);
            }
        }
    }
}
