using System.Text.Json.Serialization;

namespace PodatkovniSloj.Models;

/// <summary>
/// One national team's tournament record, as returned by <c>/{championship}/teams/results</c>.
/// </summary>
public sealed record TeamResult
{
    public int Id { get; init; }

    public required string Country { get; init; }

    public string? AlternateName { get; init; }

    public required string FifaCode { get; init; }

    public int GroupId { get; init; }

    public string GroupLetter { get; init; } = string.Empty;

    public int Wins { get; init; }

    public int Draws { get; init; }

    public int Losses { get; init; }

    public int GamesPlayed { get; init; }

    public int Points { get; init; }

    public int GoalsFor { get; init; }

    public int GoalsAgainst { get; init; }

    public int GoalDifferential { get; init; }

    /// <summary>The team as the UI lists it: "NAME (FIFA_CODE)", e.g. "Croatia (CRO)".</summary>
    [JsonIgnore]
    public string DisplayName => $"{Country} ({FifaCode})";
}
