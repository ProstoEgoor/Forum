using System;
using System.Collections.Generic;
using System.Text;
using ForumConsole.UserInterface;

namespace ForumConsole.ConsoleModel {
    public class ConsoleEventHandlerController {
        List<ConsoleEventHandler> Handlers { get; } = new List<ConsoleEventHandler>();
        public ConsoleEventHandlerController() { }
        public ConsoleEventHandlerController(IEnumerable<ConsoleEventHandler> handlers) {
            Handlers.AddRange(handlers);
        }

        public void HandleEvent(ConsoleItem consoleItem, ConsoleEventArgs e) {
            foreach (var handler in Handlers) {
                if (e.Type == handler.Type) {
                    handler?.HandleMethod(consoleItem, e);
                    break;
                }
            }
        }

        public void AddHandler(ConsoleEventHandler eventHandler) {
            Handlers.Add(eventHandler);
        }
    }
}
