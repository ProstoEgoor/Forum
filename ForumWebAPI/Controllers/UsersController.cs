using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Exceptions;

namespace ForumWebAPI.Controllers {
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly UserService userService;

        public UsersController(UserService userService) {
            this.userService = userService;
        }

        [HttpGet]
        public IAsyncEnumerable<UserProfileApiDto> Get() {
            return userService.GetProfilesAsync();
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<UserProfileApiDto>> Get([FromRoute] string userName) {
            var profile = await userService.GetProfileAsync(userName);
            if (profile == null) {
                return NotFound();
            }
            return profile;
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult> Delete([FromRoute] string userName) {
            var result = await userService.DeleteAsync(userName);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

        [HttpPost("{userName}/roles")]
        public async Task<ActionResult> PostRole([FromRoute] string userName, [FromQuery] string role) {
            var result = await userService.AssignRoleAsync(userName, role);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

        [HttpDelete("{userName}/roles")]
        public async Task<ActionResult> DeleteRole([FromRoute] string userName, [FromQuery] string role) {
            var result = await userService.RemoveFromRoleAsync(userName, role);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

        [HttpPut("{userName}")]
        public async Task<ActionResult> PutUser([FromRoute] string userName, [FromBody] UserProfileEditApiDto profile) {
            var result = await userService.UpdateProfileAsync(userName, profile);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

        [HttpPut("{userName}/password")]
        public async Task<ActionResult> ChangePassword([FromRoute] string userName, [FromBody] PasswordChangeRequestApiDto request) {
            var result = await userService.ResetPasswordAsync(userName, request.NewPassword);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

    }
}
