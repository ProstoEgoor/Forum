using ForumDbContext.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ForumDbContext.Repositories {
    public class VoteRepository : ForumRepositoryBase {
        public VoteRepository(ForumContext context) : base(context) { }

        public async Task<VoteDbDTO> GetAsync(long answerId, string authorId) {
            return await Context.Votes
                .AsQueryable()
                .Where(vote => vote.AnswerId == answerId && vote.AuthorId == authorId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool?> GetVoteResultAsync(long answerId, string authorId) {
            var voteDb = await GetAsync(answerId, authorId);
            if (voteDb != null) {
                return voteDb.Vote;
            }
            return null;
        }

        public IAsyncEnumerable<VoteDbDTO> GetAllAsync(string authorId) {
            return Context.Votes
                .AsQueryable()
                .Where(vote => vote.AuthorId == authorId)
                .AsAsyncEnumerable();
        }

        public void Create(VoteDbDTO vote) {
            Context.Votes.Add(vote);
        }

        public void Update(VoteDbDTO vote) {
            Context.Votes.Update(vote);
        }

        public void Delete(VoteDbDTO vote) {
            Context.Votes.Remove(vote);
        }
    }
}
