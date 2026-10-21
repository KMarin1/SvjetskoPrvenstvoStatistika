using System.Text.Json.Serialization;

namespace PodatkovniSloj.Models;

/// <summary>
/// One played match, as returned by <c>/{championship}/matches</c>.
/// </summary>
public sealed record Match
{
    public string Venue { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string Time { get; init; } = string.Empty;

    /// <summary>FIFA's match id. The API sends it as a string.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long FifaId { get; init; }

    public Weather? Weather { get; init; }

    /// <summary>Number of spectators. The API sends it as a string.</summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Attendance { get; init; }

    public IReadOnlyList<string> Officials { get; init; } = [];

    public string StageName { get; init; } = string.Empty;

    public string HomeTeamCountry { get; init; } = string.Empty;

    public string AwayTeamCountry { get; init; } = string.Empty;

    /// <summary>Kick-off time.</summary>
    public DateTimeOffset Datetime { get; init; }

    /// <summary>The winner's country name, or "Draw".</summary>
    public string? Winner { get; init; }

    public string? WinnerCode { get; init; }

    public required Team HomeTeam { get; init; }

    public required Team AwayTeam { get; init; }

    public IReadOnlyList<TeamEvent> HomeTeamEvents { get; init; } = [];

    public IReadOnlyList<TeamEvent> AwayTeamEvents { get; init; } = [];

    public required TeamStatistics HomeTeamStatistics { get; init; }

    public required TeamStatistics AwayTeamStatistics { get; init; }

    public DateTimeOffset? LastEventUpdateAt { get; init; }

    public DateTimeOffset? LastScoreUpdateAt { get; init; }
}
