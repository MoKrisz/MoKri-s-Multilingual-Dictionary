using Dictionary.BusinessLogic.Abstractions.Services.Synchronization;
using Dictionary.BusinessLogic.Abstractions.Tag;
using Dictionary.BusinessLogic.Behaviors;
using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.BusinessLogic.Practice.GuessArticle.Validators;
using Dictionary.BusinessLogic.Services.Synchronization;
using Dictionary.BusinessLogic.Tag.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionary.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBussinessLogic(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(GetWordHandler).Assembly);
                config.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            });

            services.AddTransient<IValidator<GetGuessArticleRandomWordsRequest>, GetGuessArticleRandomWordsRequestValidator>();

            services.AddScoped<ITagService, TagService>();

            services.AddScoped<IDataSynchronizer, DataSynchronizer>();

            return services; 
        }
    }
}
