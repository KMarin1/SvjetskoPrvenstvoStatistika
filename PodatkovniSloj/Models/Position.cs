using System.Text.Json.Serialization;

namespace PodatkovniSloj.Models;

/// <summary>A player's position in a line-up.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<Position>))]
public enum Position
{
    Goalie,
    Defender,
    Midfield,
    Forward,
}
