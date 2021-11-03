using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.FIles {
    public interface IFileEditable {
        bool Save(string path, out string error);
        bool Load(string path, out string error);
    }
}
