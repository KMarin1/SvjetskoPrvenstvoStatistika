namespace PodatkovniSloj.Tests;

/// <summary>
/// The hand-written JSON in the Fixtures folder. It follows the shape of the real API
/// responses, trimmed to a few teams, matches and players.
/// </summary>
internal static class Fixtures
{
    public const string TeamsResults = "teams-results.json";

    public const string Matches = "matches.json";

    public static string PathOf(string fileName) => Path.Combine(AppContext.BaseDirectory, "Fixtures", fileName);

    public static string Read(string fileName) => File.ReadAllText(PathOf(fileName));
}
