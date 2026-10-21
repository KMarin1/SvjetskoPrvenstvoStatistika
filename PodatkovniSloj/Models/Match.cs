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

    /// <summary>Whether the team with this FIFA code played in the match. Case does not matter.</summary>
    public bool Involves(string fifaCode) => IsHome(fifaCode) || IsAway(fifaCode);

    /// <summary>The statistics and line-up of the given team's side.</summary>
    /// <exception cref="ArgumentException">The team did not play in this match.</exception>
    public TeamStatistics StatisticsFor(string fifaCode) =>
        IsHome(fifaCode) ? HomeTeamStatistics
        : IsAway(fifaCode) ? AwayTeamStatistics
        : throw NotInMatch(fifaCode);

    /// <summary>The goals, cards and substitutions of the given team's side.</summary>
    /// <exception cref="ArgumentException">The team did not play in this match.</exception>
    public IReadOnlyList<TeamEvent> EventsFor(string fifaCode) =>
        IsHome(fifaCode) ? HomeTeamEvents
        : IsAway(fifaCode) ? AwayTeamEvents
        : throw NotInMatch(fifaCode);

    private bool IsHome(string fifaCode) => string.Equals(HomeTeam.Code, fifaCode, StringComparison.OrdinalIgnoreCase);

    private bool IsAway(string fifaCode) => string.Equals(AwayTeam.Code, fifaCode, StringComparison.OrdinalIgnoreCase);

    private ArgumentException NotInMatch(string fifaCode) =>
        new($"{fifaCode} did not play in {HomeTeam.Code}-{AwayTeam.Code}.", nameof(fifaCode));
}
