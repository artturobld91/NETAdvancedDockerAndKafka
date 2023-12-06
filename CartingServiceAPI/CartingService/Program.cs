using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using CartingService.Workers;
using CartingService.BLL.Application;
using CartingService.DAL.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CartingApi",
        Version = "v1"
    });
    options.SwaggerDoc("v2", new OpenApiInfo 
    {   Title = "CartingApi", 
        Version = "v2" 
    });

    var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
    options.IncludeXmlComments(rutaXML);
});
builder.Services.AddSingleton<MongoUnitOfWork>();
builder.Services.AddSingleton<ICartService>(x => new CartService(x.GetRequiredService<MongoUnitOfWork>()));
builder.Services.AddSingleton<IItemService>(x => new ItemService(x.GetRequiredService<MongoUnitOfWork>()));

//builder.Services.AddHostedService<KafkaConsumerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CartingApi v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "CartingApi v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
