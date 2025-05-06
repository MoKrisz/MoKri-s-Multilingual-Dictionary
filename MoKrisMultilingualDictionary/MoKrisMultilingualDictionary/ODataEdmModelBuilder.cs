using Dictionary.Models.Dtos;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace MoKrisMultilingualDictionary
{
    public static class ODataEdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<WordDto>("WordList")
                .EntityType.HasKey(w => w.WordId);

            return builder.GetEdmModel();
        }
    }
}
