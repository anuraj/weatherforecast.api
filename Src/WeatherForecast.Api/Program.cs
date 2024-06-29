using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using WeatherForecast.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey",
    options => { options.ApiKey = builder.Configuration["ApiKey"]; });

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"), new HeaderApiVersionReader("X-API-VERSION"), new MediaTypeApiVersionReader("ver"));
}).AddMvc().AddApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "X-API-KEY must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement { { key, new List<string>() } };
    options.AddSecurityRequirement(requirement);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
