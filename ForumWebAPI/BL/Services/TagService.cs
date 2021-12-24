using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;

namespace ForumWebAPI.BL.Services {
    public class TagService {
        TagRepository TagRepository { get; }

        public TagService(TagRepository tagRepository) {
            TagRepository = tagRepository;
        }

        public IAsyncEnumerable<TagFrequencyApiDto> GetFrequencyAsync() {
            return TagRepository.GetFrequencyAsync().Select(tag => new TagFrequencyApiDto(tag));
        }
    }
}
