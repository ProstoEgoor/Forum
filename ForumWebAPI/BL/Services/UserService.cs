using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Services {
    public partial class UserService {
        private readonly UserManager<UserDbDTO> userManager;
        private readonly SignInManager<UserDbDTO> signInManager;

        public UserService(UserManager<UserDbDTO> userManager, SignInManager<UserDbDTO> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        async Task<Exception> ApplyToUserAsync(string userName, Func<UserDbDTO, Task<Exception>> method) {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) {
                return new KeyNotFoundException($"Пользователь {userName} не найден.");
            }
            return await method(user);
        }

        async Task<bool> UserExistsAsync(string userName) {
            return await userManager.FindByNameAsync(userName) != null;
        }
    }
}
