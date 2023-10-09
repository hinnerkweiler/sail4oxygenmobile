using Microsoft.AspNetCore.Mvc;

namespace Sailing4oxygenApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Excelent","Good","Moderate","Challenging","Difficult","Dangerous"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<Models.WeatherForecast> Get()
    {
        //todo: DWD Seewetter OpenData for Kiel Bight
        //
        //
        return Enumerable.Range(1, 5).Select(index => new Models.WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(5, 15),
            WindspeedKts = Random.Shared.Next(5, 45),
            WinddirectionDegree = Random.Shared.Next(180,350),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

