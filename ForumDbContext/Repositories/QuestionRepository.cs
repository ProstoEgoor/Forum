using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace ForumDbContext.Repositories {
    public class QuestionRepository : ForumRepositoryBase {
        public QuestionRepository(ForumContext context) : base(context) { }
        public async Task<QuestionDbDTO> GetAsync(long questionId, bool? dateSort = null, bool ratingSort = false) {
            var question = await Context.Question
                .AsQueryable()
                .Where(question => question.QuestionId == questionId)
                .Include(question => question.Tags)
                .Include(question => question.Author)
                .AsSingleQuery()
                .FirstOrDefaultAsync();

            if (question == null) {
                return null;
            }

            var answers = Context.Entry(question)
                .Collection(question => question.Answers)
                .Query();

            if (dateSort == true) {
                answers = answers.OrderBy(answer => answer.ChangeDate);
            } else if (dateSort == false) {
                answers = answers.OrderByDescending(answer => answer.ChangeDate);
            } else if (ratingSort) {
                answers = answers.OrderByDescending(answer => answer.Rating);
            }

            await answers.Include(answer => answer.Author).LoadAsync();

            return question;
        }

        public IAsyncEnumerable<QuestionDbDTO> GetAllAsync(string authorId = null, string textSearch = null, IEnumerable<string> tagsFilter = null) {
            var questions = Context.Question.AsQueryable();

            if (authorId != null) {
                questions = questions.Where(question => question.AuthorId == authorId);
            }

            if (textSearch != null) {
                questions = questions.Where(question => EF.Functions.Like(question.Topic, $"%{textSearch}%") || EF.Functions.Like(question.QuestionText, $"%{textSearch}%"));
            }

            questions = questions.Include(question => question.Tags);

            if (tagsFilter != null) {
                //questions = questions.Where(question => tagsFilter.All(tag => question.Tags.Any(tagInQuestion => tagInQuestion.TagName == tag)));
                foreach (var tagFilter in tagsFilter) {
                    questions = questions.Where(questions => questions.Tags.Any(tag => tag.TagName == tagFilter));
                }
            }

            questions = questions
                .Include(question => question.Answers)
                .Include(question => question.Author)
                .AsSingleQuery();

            return questions.AsAsyncEnumerable();
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

        public async Task<QuestionDbDTO> DeleteAsync(long questionId) {
            var question = await Context.Question.FindAsync(questionId);

            if (question != null) {
                var entry = Context.Entry(question);
                //await entry.Collection(question => question.Tags).LoadAsync();
                //await entry.Collection(question => question.Answers).LoadAsync();
                await LoadAuthor(question);

                Delete(question);
            }

            return question;
        }

        public async Task<bool> ExistAsync(long questionId) {
            return await Context.Question.AsQueryable().AnyAsync(question => question.QuestionId == questionId);
        }

        public async Task LoadAuthor(QuestionDbDTO question) {
            await Context.Entry(question).Navigation("Author").LoadAsync();
        }
    }
}
