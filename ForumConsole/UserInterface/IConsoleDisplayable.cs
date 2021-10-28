using System.Collections.Generic;

namespace ForumConsole.UserInterface {
    public interface IConsoleDisplayable {
        void Show(int width, int indent, bool briefly);
    }
}
