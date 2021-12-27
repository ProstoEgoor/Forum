using Microsoft.AspNetCore.Identity;

namespace ForumDbContext.Model.DTO {
    public class UserDbDTO : IdentityUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
