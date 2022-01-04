using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Services;
using ForumWebAPI.BL;
using ForumWebAPI.BL.Auth;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ForumWebAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<ForumContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //options.LogTo(Console.WriteLine);
            });

            services.AddIdentity<UserDbDTO, IdentityRole>(options => {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
                .AddEntityFrameworkStores<ForumContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<UserDbDTO>, AdditionalUserClaimsPrincipalFactory>();

            services.AddForumRepositories();
            services.AddForumServices();

            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Forum API" });
            });

            services.AddScoped<IdentityDataInitializer>();
            services.AddHostedService<SetupIdentityDataInitializer>();

            services.AddAuthorization(options => {
                options.AddPolicy("EditPolicy", policy => policy.Requirements.Add(new SameAuthorRequirement(new string[] { "Moderator" })));
            });

            services.AddSingleton<IAuthorizationHandler, QuestionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AnswerAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(setup => {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum API V1");
                setup.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
