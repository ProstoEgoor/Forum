using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ForumDbContext.Repositories;

namespace ForumWebAPI.Controllers {
    [Authorize]
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase {
        private readonly AnswerRepository answerRepository;
        private readonly AnswerService answerService;
        private readonly IAuthorizationService authorizationService;

        public AnswersController(AnswerService answerService, AnswerRepository answerRepository, IAuthorizationService authorizationService) {
            this.answerService = answerService;
            this.answerRepository = answerRepository;
            this.authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerApiDto>> Get([FromRoute] long id) {
            (AnswerApiDto answer, Exception result) = await answerService.GetAsync(id);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result != null) {
                return StatusCode(500);
            }

            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerApiDto>> Post([FromBody] AnswerCreateApiDto answer) {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            (AnswerApiDto createdAnswer, Exception result) = await answerService.CreateAsync(answer, identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result != null) {
                return StatusCode(500);
            }

            return Ok(createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] long id, [FromBody] AnswerEditApiDto answer) {
            var answerToUpdate = await answerRepository.GetAsync(id);
            if (answerToUpdate == null) {
                return NotFound();
            }
            var authorizeResult = await authorizationService.AuthorizeAsync(User, answerToUpdate, "EditPolicy");
            if (!authorizeResult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }

            Exception result = await answerService.UpdateAsync(answerToUpdate, answer);

            if (result != null) {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize(Roles = "Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnswerApiDto>> Delete([FromRoute] long id) {
            var answerToDelete = await answerRepository.GetAsync(id);
            if (answerToDelete == null) {
                return NotFound();
            }
            var authorizeResult = await authorizationService.AuthorizeAsync(User, answerToDelete, "EditPolicy");
            if (!authorizeResult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }

            (AnswerApiDto deletedAnswer, Exception result) = await answerService.DeleteAsync(answerToDelete);

            if (result != null) {
                return StatusCode(500);
            }

            return Ok(deletedAnswer);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Vote([FromRoute] long id, [FromQuery] bool vote) {
            Exception result = await answerService.VoteAsync(id, vote);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result != null) {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
