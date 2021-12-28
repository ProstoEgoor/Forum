using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;

namespace ForumWebAPI.Controllers {
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase {

        QuestionService QuestionService { get; }
        AnswerService AnswerService { get; }

        public QuestionsController(QuestionService questionService, AnswerService answerService) {
            QuestionService = questionService;
            AnswerService = answerService;
        }

        [HttpGet]
        public IAsyncEnumerable<QuestionApiDto> Get([FromQuery] string text, [FromQuery] string tags) {
            return QuestionService.GetAllAsync(text, tags?.Split(','));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDetailApiDto>> Get([FromRoute] int id, [FromQuery] bool? dateSort, [FromQuery] bool ratingSort) {
            (QuestionDetailApiDto question, Exception e) = await QuestionService.GetAsync(id, dateSort, ratingSort);
            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return question;
            }
        }

        [HttpGet("{id}/answers")]
        public async Task<ActionResult<IAsyncEnumerable<AnswerApiDto>>> GetAssociatedAnswers([FromRoute] int id, [FromQuery] bool? dateSort, [FromQuery] bool ratingSort) {
            (IAsyncEnumerable<AnswerApiDto> answers, Exception e) = await AnswerService.GetAssociatedAsync(id, dateSort, ratingSort);

            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return Ok(answers);
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestionApiDto>> Post([FromBody] QuestionCreateApiDto question) {
            (QuestionApiDto CreatedQuestion, Exception e) = await QuestionService.CreateAsync(question);
            if (e != null) {
                return StatusCode(500);
            } else {
                return Ok(CreatedQuestion);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] QuestionEditApiDto question) {
            Exception e = await QuestionService.UpdateAsync(id, question);
            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDetailApiDto>> Delete([FromRoute] int id) {
            (QuestionDetailApiDto deletedQuestion, Exception e) = await QuestionService.DeleteAsync(id);

            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return deletedQuestion;
            }
        }
    }
}
