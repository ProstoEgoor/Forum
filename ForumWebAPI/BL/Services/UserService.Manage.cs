using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumWebAPI.BL.Model;
using ForumDbContext.Model.DTO;
using ForumWebAPI.BL.Exceptions;

namespace ForumWebAPI.BL.Services {
    public partial class UserService {
        public async IAsyncEnumerable<UserProfileApiDto> GetProfilesAsync() {
            var users = await userManager.Users.ToListAsync();
            foreach(var user in users) {
                yield return new UserProfileApiDto(user, await userManager.GetRolesAsync(user));
            }
        }

        public async Task<UserProfileApiDto> GetProfileAsync(string userName) {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) {
                return null;
            }
            var profile = new UserProfileApiDto(user) {
                Roles = await userManager.GetRolesAsync(user)
            };
            return profile;
        }

        public async Task<Exception> UpdateProfileAsync(UserDbDTO user, UserProfileEditApiDto profile) {
            if (user == null) {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            profile.Update(user);
            var result = await userManager.UpdateAsync(user);
            await signInManager.RefreshSignInAsync(user);
            return result.Succeeded ? null : new SaveChangesException(new Exception(result.Errors.FirstOrDefault().Description));
        }

        public async Task<Exception> UpdateProfileAsync(string userName, UserProfileEditApiDto profile) {
            return await ApplyToUserAsync(userName, (user) => UpdateProfileAsync(user, profile));
        }

        public async Task<Exception> ResetPasswordAsync(UserDbDTO user, string newPassword) {
            if (user == null) {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded ? null : new SaveChangesException(new Exception(result.Errors.FirstOrDefault().Description));
        }

        public async Task<Exception> ResetPasswordAsync(string userName, string newPassword) {
            return await ApplyToUserAsync(userName, (user) => ResetPasswordAsync(user, newPassword));
        }

        public async Task<Exception> CreateAsync(UserProfileCreateApiDto profile, IEnumerable<string> roles) {
            var user = profile.Create();
            var result = await userManager.CreateAsync(user, profile.Password);
            if (!result.Succeeded) {
                if (await UserExistsAsync(profile.UserName)) {
                    return new AlreadyExistsException($"Пользователь с именем {profile.UserName} уже существует.");
                }
                return new SaveChangesException("Не удалось создать пользователя с указанными параметрами."
                    , new Exception(result.Errors.First().Description));
            }
            result = await userManager.AddToRolesAsync(user, roles);
            if (!result.Succeeded) {
                return new SaveChangesException("Не удалось назначить пользователю одну или несколько из указанных ролей."
                    , new Exception(result.Errors.First().Description));
            }
            return null;
        }

        public async Task<Exception> DeleteAsync(UserDbDTO user) {
            if (user == null) {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded ? null : new SaveChangesException(new Exception(result.Errors.FirstOrDefault().Description));
        }

        public async Task<Exception> DeleteAsync(string userName) {
            return await ApplyToUserAsync(userName, DeleteAsync);
        }
    }
}
