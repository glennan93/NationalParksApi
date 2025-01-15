using System.Text.Json;

public interface IWeatherService
{
    Task<WeatherApiResponseDto?> GetCurrentWeatherAsync(string latitude, string longitude);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<WeatherApiResponseDto?> GetCurrentWeatherAsync(string latitude, string longitude)
    {
        var baseUrl = _configuration["Weather:BaseUrl"];

        var url = $"{baseUrl}forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m&temperature_unit=fahrenheit&wind_speed_unit=mph&precipitation_unit=inch&timezone=auto&forecast_days=1";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;  

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        var weatherDto = JsonSerializer.Deserialize<WeatherApiResponseDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return weatherDto;
    }
}
