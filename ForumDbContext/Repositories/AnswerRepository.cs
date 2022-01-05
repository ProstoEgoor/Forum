using ForumDbContext.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ForumDbContext.Repositories {
    public class AnswerRepository : ForumRepositoryBase {
        public AnswerRepository(ForumContext context) : base(context) { }

        public async Task<AnswerDbDTO> GetAsync(long answerId) {
            return await Context.Answers
                .AsQueryable()
                .Where(answer => answer.AnswerId == answerId)
                .Include(answer => answer.Author)
                .FirstOrDefaultAsync();
        }

        public IAsyncEnumerable<AnswerDbDTO> GetAssociatedAsync(long questionId, bool? dateSort, bool ratingSort) {
            var answers = Context.Answers
                .AsQueryable()
                .Where(answer => answer.QuestionId == questionId)
                .Include(answer => answer.Author)
                .AsQueryable();

            if (dateSort == true) {
                answers = answers.OrderBy(answer => answer.ChangeDate);
            } else if (dateSort == false) {
                answers = answers.OrderByDescending(answers => answers.ChangeDate);
            } else if (ratingSort) {
                answers = answers.OrderByDescending(answers => answers.Rating);
            }

            return answers.AsAsyncEnumerable();
        }

        public void Create(AnswerDbDTO answer) {
            Context.Answers.Add(answer);
        }

        public void Update(AnswerDbDTO answer) {
            Context.Answers.Update(answer);
        }

        public void Delete(AnswerDbDTO answer) {
            Context.Answers.Remove(answer);
        }

        public async Task<AnswerDbDTO> DeleteAsync(long answerId) {
            var answer = await Context.Answers.FindAsync(answerId);

            if (answer != null) {
                await LoadAuthor(answer);
                Delete(answer);
            }

            return answer;
        }

        public async Task LoadAuthor(AnswerDbDTO answer) {
            await Context.Entry(answer).Navigation("Author").LoadAsync();
        }
    }
}
