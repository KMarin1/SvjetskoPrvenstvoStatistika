using System.Globalization;
using PodatkovniSloj.Models;

namespace PodatkovniSloj.Tests;

/// <summary>Short constructors for model objects, so each test shows only the data it is about.</summary>
internal static class Build
{
    public static Team Team(string code, string? country = null) => new() { Code = code, Country = country ?? code };

    public static Match Match(
        Team home,
        Team away,
        string kickOff,
        TeamStatistics? homeStatistics = null,
        TeamStatistics? awayStatistics = null,
        IReadOnlyList<TeamEvent>? homeEvents = null,
        IReadOnlyList<TeamEvent>? awayEvents = null,
        int attendance = 0,
        string location = "Stadium") => new()
        {
            HomeTeam = home,
            AwayTeam = away,
            Datetime = DateTimeOffset.Parse(kickOff, CultureInfo.InvariantCulture),
            HomeTeamStatistics = homeStatistics ?? new TeamStatistics(),
            AwayTeamStatistics = awayStatistics ?? new TeamStatistics(),
            HomeTeamEvents = homeEvents ?? [],
            AwayTeamEvents = awayEvents ?? [],
            Attendance = attendance,
            Location = location,
        };

    public static TeamStatistics LineUp(string[] startingEleven, string[] substitutes) => new()
    {
        StartingEleven = [.. startingEleven.Select(Player)],
        Substitutes = [.. substitutes.Select(Player)],
    };

    public static Player Player(string name) => new() { Name = name };

    public static TeamEvent Event(TypeOfEvent type, string player) => new() { TypeOfEvent = type, Player = player };
}
