using Microsoft.AspNetCore.Authentication;

namespace WeatherForecast.Api
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string? ApiKey { get; set; }
    }
}
