using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterfaceNew {
    public abstract class ConsoleItem {
        public virtual bool Focusable { get; set; } = true;
        public virtual bool Adaptable { get; set; } = true;

        public event EventHandler<ConsoleEventArgs> RaiseParentEvent;

        public abstract void Show();
        public abstract bool HandleKeystroke(ConsoleKeyInfo consoleKeyInfo);

        public abstract void OnUpdate();
        
        public void OnRaiseParentEvent(object obj, ConsoleEventArgs consoleEventArgs) {
            RaiseParentEvent?.Invoke(obj, consoleEventArgs);
        }
    }
}
