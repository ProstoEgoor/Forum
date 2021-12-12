using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForumConsole.DB {
    static class ConnectionManager {
        public static string GetConnectionString(string appsettingsPath = "appsettings.json",
            string connectionStringName = "DefaultConnection",
            string environmentVaiableName = "ForumDb_ConnectionString",
            string userSecretsSection = "ForumDb") {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Program>()
                .Build();

            string userId = "", password = "", server = "";
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:UserId", out userId));
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:Password", out password));
            config.Providers.Any(p => p.TryGet($"{userSecretsSection}:Server", out server));

            return string.Format(config.GetConnectionString(connectionStringName)
                ?? Environment.GetEnvironmentVariable(environmentVaiableName),
                server,
                userId,
                password
            );
        }
    }
}
