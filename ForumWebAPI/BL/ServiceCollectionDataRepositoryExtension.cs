using ForumDbContext.Repositories;
using ForumWebAPI.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForumWebAPI.BL {
    public static class ServiceCollectionDataRepositoryExtension {

        public static void AddForumRepositories(this IServiceCollection services) {
            services.AddTransient<QuestionRepository>();
            services.AddTransient<AnswerRepository>();
            services.AddTransient<TagRepository>();
        }

        public static void AddForumServices(this IServiceCollection services) {
            services.AddTransient<QuestionService>();
            services.AddTransient<AnswerService>();
            services.AddTransient<TagService>();
            services.AddScoped<UserService>();
        }

    }
}
