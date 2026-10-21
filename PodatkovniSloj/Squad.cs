using PodatkovniSloj.Models;

namespace PodatkovniSloj;

/// <summary>A national team's players, as the project brief defines them.</summary>
public static class Squad
{
    /// <summary>
    /// Everyone in the team's first match: the starting eleven, then the substitutes.
    /// "First" means the earliest kick-off, whatever order the matches arrive in.
    /// </summary>
    /// <returns>The players, or an empty list if the team played none of <paramref name="matches"/>.</returns>
    public static IReadOnlyList<Player> FromFirstMatch(IEnumerable<Match> matches, string fifaCode)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentException.ThrowIfNullOrWhiteSpace(fifaCode);

        var firstMatch = matches.Where(match => match.Involves(fifaCode)).MinBy(match => match.Datetime);
        if (firstMatch is null)
        {
            return [];
        }

        var side = firstMatch.StatisticsFor(fifaCode);
        return [.. side.StartingEleven.Concat(side.Substitutes).DistinctBy(player => player.Name)];
    }
}
