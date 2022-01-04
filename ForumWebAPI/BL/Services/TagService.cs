using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;

namespace ForumWebAPI.BL.Services {
    public class TagService {
        private readonly TagRepository tagRepository;

        public TagService(TagRepository tagRepository) {
            this.tagRepository = tagRepository;
        }

        public IAsyncEnumerable<TagFrequencyApiDto> GetFrequencyAsync() {
            return tagRepository.GetFrequencyAsync().Select(tag => new TagFrequencyApiDto(tag));
        }
    }
}
