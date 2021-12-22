using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace ForumDbContext.Repositories {
    class QuestionRepository : ForumRepositoryBase {
        public QuestionRepository(ForumContext context) : base(context) { }

        public IAsyncEnumerable<QuestionDbDTO> GetAll(string textSearch = null, IEnumerable<string> tagsFilter = null) {
            var questions = Context.Question.AsQueryable();

            if (textSearch != null) {
                questions = questions.Where(question => EF.Functions.Like(question.QuestionText, $"%{textSearch}%"));
            }

            questions = questions.Include(question => question.Tags);

            if (tagsFilter != null) {
                questions = questions.Where(question => tagsFilter.All(tag => question.Tags.Any(tagInQuestion => tagInQuestion.TagName == tag)));
            }

            return questions.AsAsyncEnumerable();
        }

        public async Task<QuestionDbDTO> GetAsync(int questionId, bool? dateSort, bool ratingSort) {
            var question = await Context.Question
                .AsQueryable()
                .Where(question => question.QuestionId == questionId)
                .Include(question => question.Tags)
                .FirstAsync();

            var answers = Context.Entry(question)
                .Collection(question => question.Answers)
                .Query();

            if (dateSort == true) {
                answers = answers.OrderBy(answer => answer.CreateDate);
            } else if (dateSort == false) {
                answers = answers.OrderByDescending(answer => answer.CreateDate);
            } else if (ratingSort) {
                answers = answers.OrderByDescending(answer => answer.Rating);
            }

            await answers.LoadAsync();

            return question;
        }

        public void Create(QuestionDbDTO question) {
            Context.Question.Add(question);
        }

        public void Update(QuestionDbDTO question) {
            Context.Question.Update(question);
        }

        public void Delete(QuestionDbDTO question) {
            Context.Question.Remove(question);
        }

        public async Task<QuestionDbDTO> DeleteAsync(int id) {
            var question = await Context.Question.FindAsync(id);

            if (question != null) {
                Delete(question);
            }

            return question;
        }
    }
}
