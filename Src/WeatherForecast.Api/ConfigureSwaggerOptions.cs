using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace WeatherForecast.Api
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = "WeatherForecast.Api",
                    Version = description.ApiVersion.ToString(),
                    Description = "API endpoints for WeatherForecast"
                };

                options.SwaggerDoc(description.GroupName, info);
            }
        }
    }
}
