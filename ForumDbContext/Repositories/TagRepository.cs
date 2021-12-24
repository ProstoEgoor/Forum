using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ForumDbContext.Repositories {
    public class TagRepository : ForumRepositoryBase {
        public TagRepository(ForumContext context) : base(context) { }

        public IAsyncEnumerable<TagInQuestionDbDTO> GetAssociatedAsync(int questionId) {
            return Context.TagInQuestions
                .AsQueryable()
                .Where(tag => tag.QuestionId == questionId)
                .AsAsyncEnumerable();
        }

        public IAsyncEnumerable<TagFrequencyDbDTO> GetFrequencyAsync() {
            return Context.TagsFrequency
                .AsQueryable()
                .OrderByDescending(tag => tag.Frequency)
                .AsAsyncEnumerable();
        }
    }
}
