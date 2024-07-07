using Dictionary.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.Tests.IntegrationTests.WebApi
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactoryWithDbContext<Program>>
    {
        private readonly WebApplicationFactoryWithDbContext<Program> _factory;
        protected readonly HttpClient _client;

        public IntegrationTestBase(WebApplicationFactoryWithDbContext<Program> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }

        protected DictionaryContext GetDatabase()
        {
            var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DictionaryContext>();
            db.Database.EnsureCreated();
            return db;
        }
    }
}
