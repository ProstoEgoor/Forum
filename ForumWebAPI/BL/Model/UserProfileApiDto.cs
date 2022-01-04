using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class UserProfileEditApiDto {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual void Update(UserDbDTO user) {
            user.Email = Email ?? user.Email;
            user.FirstName = FirstName ?? user.FirstName;
            user.LastName = LastName ?? user.LastName;
        }
    }

    public class UserProfileCreateApiDto : UserProfileEditApiDto {
        public static string[] DefaultRoles = new string[] { "User" };
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual UserDbDTO Create() {
            return new UserDbDTO() {
                UserName = UserName,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
            };
        }
    }

    public class UserProfileApiDto : UserProfileEditApiDto {
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public UserProfileApiDto() { }

        public UserProfileApiDto(UserDbDTO user) {
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public UserProfileApiDto(UserDbDTO user, IEnumerable<string> roles) : this(user) {
            Roles = roles;
        }
    }
}
