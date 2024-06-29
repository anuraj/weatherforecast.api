using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
[ApiVersion(3.0)]
[Authorize]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [MapToApiVersion(2.0)]
    [HttpGet("{id}", Name = "GetWeatherForecastById")]
    public WeatherForecast Get(int id)
    {
        var rng = new Random();
        return new WeatherForecast
        {
            Date = DateTime.Now.AddDays(id),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        };
    }

    [HttpPost]
    [ApiVersion(1.0, Deprecated = true)]
    public IActionResult Post([FromBody] WeatherForecast weatherForecast)
    {
        return CreatedAtRoute("GetWeatherForecastById", new { id = 1 }, weatherForecast);
    }
}
