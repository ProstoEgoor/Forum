using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public interface IConsoleDisplayableBriefly : IConsoleDisplayable {
        void Show((int left, int right) indent, bool briefly);
    }
}
