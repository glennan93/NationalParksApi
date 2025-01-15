public class WeatherApiResponseDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double GenerationtimeMs { get; set; }
    public int UtcOffsetSeconds { get; set; }
    public string Timezone { get; set; }
    public string TimezoneAbbreviation { get; set; }
    public double Elevation { get; set; }
    public CurrentUnitsDto Current_Units { get; set; }
    public CurrentWeatherDto Current { get; set; }
}

public class CurrentUnitsDto
{
    public string Time { get; set; }
    public string Interval { get; set; }
    public string Temperature_2m { get; set; }
}

public class CurrentWeatherDto
{
    public string Time { get; set; }
    public int Interval { get; set; }
    public double Temperature_2m { get; set; }
}
