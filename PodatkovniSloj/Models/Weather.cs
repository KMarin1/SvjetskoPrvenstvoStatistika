using System.Text.Json.Serialization;

namespace PodatkovniSloj.Models;

/// <summary>Weather at kick-off. The API sends every number here as a string.</summary>
[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public sealed record Weather
{
    public int Humidity { get; init; }

    public int TempCelsius { get; init; }

    /// <summary>Spelled as the API spells it (<c>temp_farenheit</c>).</summary>
    public int TempFarenheit { get; init; }

    public int WindSpeed { get; init; }

    public string Description { get; init; } = string.Empty;
}
