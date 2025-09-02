using Dictionary.BusinessLogic.Services.Synchronization;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IDataSynchronizer, DataSynchronizer>();

            return services; 
        }
    }
}
