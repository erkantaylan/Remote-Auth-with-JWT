using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Weather.Api.Identity;

namespace Weather.Api.Weather;

[Route("weatherforecast")]
[ApiController]
public class WeatherController : Controller
{
    private readonly string[] summaries =
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };
    
    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPost]
    public IActionResult GetWeatherForecast()
    {
        WeatherForecast Selector(int index)
        {
            return new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]);
        }

        WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(Selector).ToArray();
        return new JsonResult(forecast);
    }
}