using System.Collections.Generic;

namespace ForumConsole.UserInterface {
    public interface IConsolePrintable {
        void Print(int width, int indent, bool briefly);
    }
}
