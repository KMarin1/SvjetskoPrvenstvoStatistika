using PodatkovniSloj.Models;

namespace PodatkovniSloj;

/// <summary>
/// The rankings the project brief asks for, all for one national team. Each one is computed
/// from the match events, so it covers every match passed in.
/// </summary>
public static class Rankings
{
    private static readonly HashSet<TypeOfEvent> GoalEvents = [TypeOfEvent.Goal, TypeOfEvent.GoalPenalty];

    private static readonly HashSet<TypeOfEvent> YellowCardEvents = [TypeOfEvent.YellowCard, TypeOfEvent.YellowCardSecond];

    /// <summary>
    /// The team's scorers, most goals first, ties by name. Penalties count. Own goals do not:
    /// the API lists them under the scorer's own team, but they count for the opponent.
    /// </summary>
    public static IReadOnlyList<PlayerTally> Goals(IEnumerable<Match> matches, string fifaCode) =>
        Tally(matches, fifaCode, GoalEvents);

    /// <summary>The team's players by yellow cards, most first, ties by name. A second yellow counts as a yellow card.</summary>
    public static IReadOnlyList<PlayerTally> YellowCards(IEnumerable<Match> matches, string fifaCode) =>
        Tally(matches, fifaCode, YellowCardEvents);

    /// <summary>The team's matches by attendance, largest crowd first; equal crowds in kick-off order.</summary>
    public static IReadOnlyList<AttendanceEntry> Attendance(IEnumerable<Match> matches, string fifaCode)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentException.ThrowIfNullOrWhiteSpace(fifaCode);

        return [.. matches
            .Where(match => match.Involves(fifaCode))
            .OrderByDescending(match => match.Attendance)
            .ThenBy(match => match.Datetime)
            .Select(match => new AttendanceEntry(match.Location, match.Attendance, match.HomeTeam.Country, match.AwayTeam.Country))];
    }

    private static List<PlayerTally> Tally(IEnumerable<Match> matches, string fifaCode, HashSet<TypeOfEvent> counted)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentException.ThrowIfNullOrWhiteSpace(fifaCode);

        return [.. matches
            .Where(match => match.Involves(fifaCode))
            .SelectMany(match => match.EventsFor(fifaCode))
            .Where(teamEvent => counted.Contains(teamEvent.TypeOfEvent))
            .GroupBy(teamEvent => teamEvent.Player, StringComparer.Ordinal)
            .Select(group => new PlayerTally(group.Key, group.Count()))
            .OrderByDescending(tally => tally.Count)
            .ThenBy(tally => tally.PlayerName, StringComparer.Ordinal)];
    }
}
