using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using System.Threading.Tasks;

namespace ForumDbContext.Repositories {
    class TagRepository : ForumRepositoryBase {
        public TagRepository(ForumContext context) : base(context) { }

        public IAsyncEnumerable<TagInQuestionDbDTO> GetTags(int questionId) {
            return Context.TagInQuestions
                .AsQueryable()
                .Where(tag => tag.QuestionId == questionId)
                .ToAsyncEnumerable();
        }

        public IAsyncEnumerable<TagFrequencyDbDTO> GetTagFrequency() {
            return Context.TagsFrequency
                .AsQueryable()
                .OrderByDescending(tag => tag.Frequency)
                .ToAsyncEnumerable();
        }
    }
}
