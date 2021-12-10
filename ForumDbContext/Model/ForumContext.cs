using System;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Connection;
using ForumDbContext.Model.Configure;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace ForumDbContext.Model {
    public class ForumContext : DbContext {
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
            modelBuilder.ApplyConfiguration(new QuestionDbConfig());
            modelBuilder.ApplyConfiguration(new AnswerDbConfig());
            modelBuilder.ApplyConfiguration(new TagInQuestionDbConfig());
            modelBuilder.ApplyConfiguration(new TagFrequencyDbConfig());
        }
    }
}
