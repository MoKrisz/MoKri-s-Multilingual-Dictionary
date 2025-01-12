using Dictionary.BusinessLogic;
using Dictionary.Data;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MoKrisMultilingualDictionary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DictionaryContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetWordHandler).Assembly));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
        policy.WithOrigins("http://localhost:5173")
            .WithHeaders(HeaderNames.ContentType)
            .WithMethods("POST", "PUT")
            );
});

var odataEdmModel = ODataEdmModelBuilder.GetEdmModel();
builder.Services.AddControllers()
    .AddOData(
        options => options.Select()
                          .Filter()
                          .OrderBy()
                          .Expand()
                          .Count()
                          .SetMaxTop(100)
                          .AddRouteComponents("odata", odataEdmModel)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }