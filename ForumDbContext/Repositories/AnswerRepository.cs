using ForumDbContext.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ForumDbContext.Repositories {
    class AnswerRepository : ForumRepositoryBase {
        public AnswerRepository(ForumContext context) : base(context) { }

        public IAsyncEnumerable<AnswerDbDTO> GetAnswers(int questionId, bool? dateSort, bool ratingSort) {
            var answers = Context.Answers
                .AsQueryable()
                .Where(answer => answer.QuestionId == questionId);

            if (dateSort == true) {
                answers = answers.OrderBy(answer => answer.CreateDate);
            } else if (dateSort == false) {
                answers = answers.OrderByDescending(answers => answers.CreateDate);
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

        public async Task<AnswerDbDTO> DeleteAsync(int id) {
            var answer = await Context.Answers.FindAsync(id);

            if (answer != null) {
                Delete(answer);
            }

            return answer;
        }
    }
}
