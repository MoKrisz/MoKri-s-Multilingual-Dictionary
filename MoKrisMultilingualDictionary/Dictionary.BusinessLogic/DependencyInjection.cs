using Dictionary.BusinessLogic.Abstractions.Services.Synchronization;
using Dictionary.BusinessLogic.Abstractions.Tag;
using Dictionary.BusinessLogic.Services.Synchronization;
using Dictionary.BusinessLogic.Tag.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ITagService, TagService>();

            services.AddScoped<IDataSynchronizer, DataSynchronizer>();

            return services; 
        }
    }
}
