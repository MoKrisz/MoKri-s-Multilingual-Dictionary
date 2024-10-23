using Dictionary.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace Dictionary.Tests.IntegrationTests.WebApi
{
    public class IntegrationTestFixture<TProgram>
        : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
    {
        protected readonly PostgreSqlContainer postgresContainer;
        private Respawner respawner;
        private DbConnection connection;
        private DictionaryContext dbContext;

        public HttpClient Client { get; private set; }

        public IntegrationTestFixture()
        {
            postgresContainer = new PostgreSqlBuilder().Build();
            Client = CreateClient();
        }

        public async Task InitializeAsync()
        {
            await postgresContainer.StartAsync();

            var scope = Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<DictionaryContext>();
            dbContext.Database.EnsureCreated();

            connection = dbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "dictionary" }
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DictionaryContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<DictionaryContext>((options) =>
                {
                    options.UseNpgsql(postgresContainer.GetConnectionString());
                });
            });

            builder.UseEnvironment("Development");
        }

        public async Task<DictionaryContext> GetDatabase()
        {
            await ResetDatabase();
            return dbContext;
        }

        public async Task ResetDatabase()
        {
            await respawner.ResetAsync(connection);
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await connection.CloseAsync();
            await postgresContainer.DisposeAsync();
        }
    }
}
