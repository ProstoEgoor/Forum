using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;

namespace ForumWebAPI.Controllers {
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase {
        AnswerService AnswerService { get; }

        public AnswersController(AnswerService answerService) {
            AnswerService = answerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerApiDto>> Get([FromRoute] int id) {
            (AnswerApiDto answer, Exception e) = await AnswerService.GetAsync(id);

            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return Ok(answer);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AnswerApiDto>> Post([FromBody] AnswerApiDto answer) {
            (AnswerApiDto createdAnswer, Exception e) = await AnswerService.CreateAsync(answer);

            if (e != null) {
                if (e is KeyNotFoundException) {
                    return BadRequest(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return Ok(createdAnswer);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] AnswerApiDto answer) {
            Exception e = await AnswerService.UpdateAsync(id, answer);

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
        public async Task<ActionResult<AnswerApiDto>> Delete([FromRoute] int id) {
            (AnswerApiDto deletedAnswer, Exception e) = await AnswerService.DeleteAsync(id);

            if (e != null) {
                if (e is KeyNotFoundException) {
                    return NotFound(e.Message);
                } else {
                    return StatusCode(500);
                }
            } else {
                return deletedAnswer;
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Vote([FromRoute] int id, [FromQuery] bool vote) {
            Exception e = await AnswerService.VoteAsync(id, vote);

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
    }
}
