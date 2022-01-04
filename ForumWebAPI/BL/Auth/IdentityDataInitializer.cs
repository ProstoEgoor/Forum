using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ForumDbContext.Model.DTO;
using System.Threading;

namespace ForumWebAPI.BL.Auth {
    public class IdentityDataInitializer {
        readonly IConfiguration configuration;
        readonly RoleManager<IdentityRole> roleManager;
        readonly UserManager<UserDbDTO> userManager;

        public IdentityDataInitializer(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<UserDbDTO> userManager) {
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task InitializeAsync() {
            var roles = configuration
                .GetSection("Roles")
                .GetChildren()
                .Select(section => section.Value);

            foreach (var role in roles) {
                await AddRole(role);
            }

            var defaultUsers = configuration
                .GetSection("DefaultUsers")
                .GetChildren();

            foreach (var user in defaultUsers) {
                await AddUser(user["Username"], user["Email"], user["Password"], user["Roles"].Split(","));
            }
        }

        async Task AddRole(string role) {
            if (!await roleManager.RoleExistsAsync(role)) {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        async Task AddUser(string userТame, string email, string password, IEnumerable<string> roles) {
            var user = await userManager.FindByNameAsync(userТame);
            if (user == null) {
                user = new UserDbDTO() {
                    UserName = userТame,
                    Email = email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, password);
            }

            foreach (var role in roles) {
                await AddUserToRole(user, role);
            }
        }

        async Task AddUserToRole(UserDbDTO user, string role) {
            if (user != null && !await userManager.IsInRoleAsync(user, role)) {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }

    public class SetupIdentityDataInitializer : IHostedService {

        readonly IServiceProvider serviceProvider;

        public SetupIdentityDataInitializer(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            using var scope = serviceProvider.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<IdentityDataInitializer>();
            await initializer.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
