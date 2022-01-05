using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumWebAPI.BL.Exceptions {
    public class AlreadyVotesException : Exception {
        public AlreadyVotesException(string message) : base(message) { }
    }
}
