using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ForumWebAPI.BL.Services;
using ForumWebAPI.BL.Model;
using ForumDbContext.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ForumWebAPI.BL.Exceptions;

namespace ForumWebAPI.Controllers {
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase {
        private readonly UserService userService;
        private readonly SignInManager<UserDbDTO> signInManager;

        public AccountController(UserService userService, SignInManager<UserDbDTO> signInManager) {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestApiDto request) {
            var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false);

            if (!result.Succeeded) {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost("logout")]
        public async Task Logout() {
            await signInManager.SignOutAsync();
        }

        [HttpGet]
        public UserProfileApiDto Get() {
            var identity = User.Identity as ClaimsIdentity;
            var profile = new UserProfileApiDto() {
                UserName = identity.Name,
                Email = identity.FindFirst(ClaimTypes.Email)?.Value,
                FirstName = identity.FindFirst("FirstName")?.Value,
                LastName = identity.FindFirst("LastName")?.Value,
                Roles = identity.FindAll(ClaimTypes.Role).Select(role => role.Value)
            };

            return profile;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserProfileCreateApiDto profile) {
            var result = await userService.CreateAsync(profile, UserProfileCreateApiDto.DefaultRoles);

            if (result is AlreadyExistsException) {
                return Conflict(result.Message);
            } else if (result is SaveChangesException) {
                return BadRequest($"{result.Message} \n {result.InnerException.Message}");
            } else if (result != null) {
                return StatusCode(500, result.Message);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserProfileEditApiDto profile) {
            var result = await userService.UpdateProfileAsync(HttpContext.User.Identity.Name, profile);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result is SaveChangesException) {
                return BadRequest($"{result.Message} \n {result.InnerException.Message}");
            } else if (result != null) {
                return StatusCode(500, result.Message);
            }

            return Ok();
        }

        [HttpPut("password")]
        public async Task<ActionResult> PutPassword([FromBody] PasswordChangeRequestApiDto request) {
            var result = await userService.ResetPasswordAsync(HttpContext.User.Identity.Name, request.NewPassword);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result is SaveChangesException) {
                return BadRequest($"{result.Message} \n {result.InnerException.Message}");
            } else if (result != null) {
                return StatusCode(500, result.Message);
            }

            return Ok();
        }
    }
}
