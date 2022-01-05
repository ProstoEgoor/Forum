using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ForumWebAPI.BL.Auth {
    public class SameAuthorRequirement : IAuthorizationRequirement {
        public IEnumerable<string> RolesThatCanModify { get; set; }

        public SameAuthorRequirement(IEnumerable<string> rolesThatCanModify) {
            RolesThatCanModify = rolesThatCanModify;
        }
    }
}
