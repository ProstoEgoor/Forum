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
using ForumWebAPI.BL.Exceptions;

namespace ForumWebAPI.Controllers {
    [Authorize]
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase {
        private readonly AnswerRepository answerRepository;
        private readonly AnswerService answerService;
        private readonly VoteService voteService;
        private readonly IAuthorizationService authorizationService;

        public AnswersController(AnswerService answerService, AnswerRepository answerRepository, IAuthorizationService authorizationService, VoteService voteService) {
            this.answerService = answerService;
            this.answerRepository = answerRepository;
            this.authorizationService = authorizationService;
            this.voteService = voteService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerApiDto>> Get([FromRoute] long id) {
            (AnswerApiDto answer, Exception result) = await answerService.GetAsync(id);

            if (result != null) {
                return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
            }

            if (User.Identity.IsAuthenticated) {
                answer = await voteService.IncludeVoteAsync(answer, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerApiDto>> Post([FromBody] AnswerCreateApiDto answer) {
            (AnswerApiDto createdAnswer, Exception result) = await answerService.CreateAsync(answer, User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}", createdAnswer);
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

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }

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

            (AnswerApiDto deletedAnswer, Exception result) = await answerService.DeleteAsync(id);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}", deletedAnswer);
        }

        [HttpPost("{id}/vote")]
        public async Task<ActionResult> Vote([FromRoute] long id, [FromQuery] bool? vote) {
            Exception result = await voteService.VoteAsync(id, vote, User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return result.GetResultObject($"{result?.Message}. {result?.InnerException?.Message}");
        }
    }
}
