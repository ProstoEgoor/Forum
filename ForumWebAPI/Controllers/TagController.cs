using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;

namespace ForumWebAPI.Controllers {
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase {
        TagService TagService { get; }

        public TagController(TagService tagService) {
            TagService = tagService;
        }

        [HttpGet]
        public IAsyncEnumerable<TagFrequencyApiDto> Get() {
            return TagService.GetFrequencyAsync();
        }
    }
}
