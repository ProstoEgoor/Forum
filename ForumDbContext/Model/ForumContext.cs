using System;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Connection;
using ForumDbContext.Model.Configure;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ForumDbContext.Model {
    public class ForumContext : IdentityDbContext<UserDbDTO> {
        public ForumContext() { }
        public ForumContext(DbContextOptions options) : base(options) { }

        public DbSet<QuestionDbDTO> Question { get; set; }
        public DbSet<AnswerDbDTO> Answers { get; set; }
        public DbSet<TagInQuestionDbDTO> TagInQuestions { get; set; }
        public DbSet<TagFrequencyDbDTO> TagsFrequency { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(new ConnectionStringConfiguration().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new QuestionDbConfig());
            modelBuilder.ApplyConfiguration(new AnswerDbConfig());
            modelBuilder.ApplyConfiguration(new TagInQuestionDbConfig());
            modelBuilder.ApplyConfiguration(new TagFrequencyDbConfig());
            modelBuilder.ApplyConfiguration(new UserDbConfig());
        }

        public void Replace<TEntity>(TEntity oldEntity, TEntity newEntity) where TEntity : class {
            ChangeTracker.TrackGraph(oldEntity, e => e.Entry.State = EntityState.Deleted);
            ChangeTracker.TrackGraph(newEntity, e => e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added);
        }
    }
}
