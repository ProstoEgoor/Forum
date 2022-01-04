using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Services;
using Microsoft.AspNetCore.Authorization;

namespace ForumWebAPI.Controllers {
    [AllowAnonymous]
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase {
        private readonly TagService tagService;

        public TagController(TagService tagService) {
            this.tagService = tagService;
        }

        [HttpGet]
        public IAsyncEnumerable<TagFrequencyApiDto> Get() {
            return tagService.GetFrequencyAsync();
        }
    }
}
