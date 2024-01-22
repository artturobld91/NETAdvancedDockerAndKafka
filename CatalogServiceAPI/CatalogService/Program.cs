
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string kafkaContainerUrl = builder.Configuration["Kafka"].ToString();
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 1. Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-5ee896fp.us.auth0.com/";
    options.Audience = "https://localhost";
});


builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Manager",
        policy => policy.RequireClaim("scope", "manager:catalog"));

        options.AddPolicy("Buyer",
          policy => policy.RequireClaim("scope", "buyer:catalog"));

        options.AddPolicy("Manager, Buyer",
          policy => policy.RequireClaim("scope", "manager:catalog", "buyer:catalog"));
    }
);


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<CatalogService.Services.LinkGenerator>();
builder.Services.AddTransient<CatalogService.Filters.HateoasCategoryFilterAttribute>();
builder.Services.AddTransient<CatalogService.Filters.HateoasItemFilterAttribute>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
