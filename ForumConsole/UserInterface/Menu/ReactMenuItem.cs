using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public class ReactMenuItem : MenuItem, IConsoleReactive {
        public ConsoleEvent Type { get; }

        public event EventHandler<ConsoleEventArgs> RaiseEvent;

        public ReactMenuItem(ConsoleEvent type, ConsoleKeyInfo keyInfo, string keyTitle, string description, int order = 1) : base(keyInfo, keyTitle, description, order) {
            Type = type;
        }

        public ReactMenuItem(ConsoleEvent type, ConsoleKeyInfo keyInfo, string description, int order = 1) : this(type, keyInfo, keyInfo.Key.ToString(), description, order) { }

        public virtual bool HandlePressedKey(ConsoleKeyInfo keyInfo) {
            if (KeyInfo == keyInfo) {
                OnRaiseEvent(new ConsoleEventArgs(Type));
                return true;
            }

            return false;
        }
        public void HandleEvent(object obj, ConsoleEventArgs consoleEvent) {
            OnRaiseEvent(consoleEvent);
        }

        public void OnRaiseEvent(ConsoleEventArgs consoleEvent) {
            RaiseEvent?.Invoke(this, consoleEvent);
        }

    }
}
