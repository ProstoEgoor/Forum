using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterfaceNew {
    public class ConsoleView {
        protected ConsoleView Prev { get; }
        public ConsoleView Next { get; set; }

        public ConsoleItem Container { get; }

        public ConsoleEventHandler ConsoleEventHandler { get; }

        public ConsoleView(ConsoleView prev, ConsoleItem container) {
            Prev = prev;
            Next = this;
            Container = container;
            Container.RaiseParentEvent += HandleConsoleEvent;
            ConsoleEventHandler = new ConsoleEventHandler();
        }

        public void ShowView() {
            Console.Clear();
            Container.Show();
        }

        public void HandleKeystroke(ConsoleKeyInfo consoleKeyInfo) {
            if (Container.Focusable) {
                Container.HandleKeystroke(consoleKeyInfo);
            }
        }

        public void HandleConsoleEvent(object sender, ConsoleEventArgs consoleEventArgs) {
            ConsoleEventHandler.HandleConsoleEvent(this, sender as ConsoleItem, consoleEventArgs);
        }

        public virtual void OnUpdate() {
            if (Container.Adaptable) {
                Container.OnUpdate();
            }
        }
        public virtual void OnStart() {
        }
        public virtual void OnResume() {
        }
        public virtual void OnPause() {
        }
    }
}
