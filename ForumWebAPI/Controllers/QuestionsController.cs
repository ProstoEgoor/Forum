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
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase {
        private readonly QuestionRepository questionRepository;
        private readonly QuestionService questionService;
        private readonly AnswerService answerService;
        private readonly IAuthorizationService authorizationService;

        public QuestionsController(QuestionService questionService, AnswerService answerService, IAuthorizationService authorizationService, QuestionRepository questionRepository) {
            this.questionService = questionService;
            this.answerService = answerService;
            this.authorizationService = authorizationService;
            this.questionRepository = questionRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IAsyncEnumerable<QuestionApiDto> Get([FromQuery] string text, [FromQuery] string tags) {
            return questionService.GetAllAsync(text, tags?.Split(','));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDetailApiDto>> Get([FromRoute] long id, [FromQuery] bool? dateSort, [FromQuery] bool ratingSort) {
            (QuestionDetailApiDto question, Exception result) = await questionService.GetAsync(id, dateSort, ratingSort);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result != null) {
                return StatusCode(500);
            }

            return question;

        }

        [AllowAnonymous]
        [HttpGet("{id}/answers")]
        public async Task<ActionResult<IAsyncEnumerable<AnswerApiDto>>> GetAssociatedAnswers([FromRoute] long id, [FromQuery] bool? dateSort, [FromQuery] bool ratingSort) {
            (IAsyncEnumerable<AnswerApiDto> answers, Exception result) = await answerService.GetAssociatedAsync(id, dateSort, ratingSort);

            if (result is KeyNotFoundException) {
                return NotFound(result.Message);
            } else if (result != null) {
                return StatusCode(500);
            }

            return Ok(answers);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionApiDto>> Post([FromBody] QuestionCreateApiDto question) {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            (QuestionApiDto CreatedQuestion, Exception result) = await questionService.CreateAsync(question, identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (result != null) {
                return StatusCode(500);
            }

            return Ok(CreatedQuestion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] long id, [FromBody] QuestionEditApiDto question) {
            var questionToUpdate = await questionRepository.GetAsync(id);
            if (questionToUpdate == null) {
                return NotFound();
            }
            var authorizeResult = await authorizationService.AuthorizeAsync(User, questionToUpdate, "EditPolicy");
            if (!authorizeResult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }
            Exception result = await questionService.UpdateAsync(questionToUpdate, question);

            if (result != null) {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDetailApiDto>> Delete([FromRoute] long id) {
            var questionToDelete = await questionRepository.GetAsync(id);
            if (questionToDelete == null) {
                return NotFound();
            }
            var authorizeResult = await authorizationService.AuthorizeAsync(User, questionToDelete, "EditPolicy");
            if (!authorizeResult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }
            (QuestionDetailApiDto deletedQuestion, Exception result) = await questionService.DeleteAsync(id);

            if (result != null) {
                return StatusCode(500);
            }

            return deletedQuestion;
        }
    }
}
