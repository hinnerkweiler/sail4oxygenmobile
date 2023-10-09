namespace Sailing4oxygenApi.Models;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public int WindspeedKts { get; set; }

    public int WinddirectionDegree { get; set; }

    public double WavehightMeter => WindspeedKts / 10;

    public string? Summary { get; set; }
}

