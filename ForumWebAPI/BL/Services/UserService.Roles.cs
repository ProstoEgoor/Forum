using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;
using ForumWebAPI.BL.Exceptions;

namespace ForumWebAPI.BL.Services {
    public partial class UserService {
        public async Task<Exception> AssignRoleAsync(UserDbDTO user, string role, bool updateCookies) {
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) {
                return new SaveChangesException($"Не удалось назначить пользователю {user.UserName} роль {role}");
            }
            if (updateCookies) {
                await signInManager.RefreshSignInAsync(user);
            }
            return null;
        }

        public async Task<Exception> AssignRoleAsync(string userName, string role, bool updateCookies = false) {
            return await ApplyToUserAsync(userName, user => AssignRoleAsync(user, role, updateCookies));
        }

        public async Task<Exception> RemoveFromRoleAsync(UserDbDTO user, string role, bool updateCookies) {
            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded) {
                return new SaveChangesException($"Не удалось удалить у пользователя {user.UserName} роль {role}");
            }

            if (updateCookies) {
                await signInManager.RefreshSignInAsync(user);
            }

            return null;
        }

        public async Task<Exception> RemoveFromRoleAsync(string userName, string role, bool updateCookies = false) {
            return await ApplyToUserAsync(userName, user => RemoveFromRoleAsync(user, role, updateCookies));
        }

        public async Task<Exception> SetRoles(UserDbDTO user, IEnumerable<string> roles, bool updateCookies) {
            var result = await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
            if (result.Succeeded) {
                result = await userManager.AddToRolesAsync(user, roles);
            }

            if (!result.Succeeded) {
                return new SaveChangesException($"Не удалось назначить пользователю {user.UserName} роли");
            }

            if (updateCookies) {
                await signInManager.RefreshSignInAsync(user);
            }

            return null;
        }

        public async Task<Exception> SetRoles(string userName, IEnumerable<string> roles, bool updateCookies = false) {
            return await ApplyToUserAsync(userName, user => SetRoles(user, roles, updateCookies));
        }
    }
}
