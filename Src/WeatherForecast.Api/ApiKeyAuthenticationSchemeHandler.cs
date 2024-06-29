using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WeatherForecast.Api
{
public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    public ApiKeyAuthenticationSchemeHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));
        }

        var apiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));
        }

        if (apiKey != Options.ApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "API User") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
}
