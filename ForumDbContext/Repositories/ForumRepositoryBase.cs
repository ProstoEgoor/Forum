using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ForumDbContext.Model;

namespace ForumDbContext.Repositories {
    class ForumRepositoryBase {
        protected ForumContext Context { get; }

        public ForumRepositoryBase(ForumContext context) {
            Context = context;
        }

        public async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }
    }
}
