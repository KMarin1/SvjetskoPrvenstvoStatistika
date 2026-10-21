namespace PodatkovniSloj.Tests;

public sealed class JsonFileServiceTests : IDisposable
{
    private readonly TempDirectory _root = new();

    private static CancellationToken TestCancellation => TestContext.Current.CancellationToken;

    public void Dispose() => _root.Dispose();

    [Fact]
    public async Task Reads_teams_and_matches_from_files_laid_out_like_the_api_paths()
    {
        SaveFixture(Fixtures.TeamsResults, "women", "teams", "results.json");
        SaveFixture(Fixtures.Matches, "women", "matches.json");
        var source = new JsonFileService(_root.Path);

        var teams = await source.GetTeamsAsync(Championship.Women, TestCancellation);
        var matches = await source.GetMatchesAsync(Championship.Women, TestCancellation);

        string[] expectedCodes = ["KOR", "CRO"];
        Assert.Equal(expectedCodes, teams.Select(team => team.FifaCode));
        Assert.Equal(2, matches.Count);
    }

    [Theory]
    [InlineData("NGA", 1)]
    [InlineData("cro", 2)]
    [InlineData("BRA", 0)]
    public async Task GetTeamMatchesAsync_keeps_only_that_teams_matches(string fifaCode, int expectedCount)
    {
        SaveFixture(Fixtures.Matches, "men", "matches.json");

        var matches = await new JsonFileService(_root.Path).GetTeamMatchesAsync(Championship.Men, fifaCode, TestCancellation);

        Assert.Equal(expectedCount, matches.Count);
        Assert.All(matches, match => Assert.True(match.Involves(fifaCode)));
    }

    [Fact]
    public async Task A_missing_file_becomes_a_WorldCupDataException_naming_the_file()
    {
        var source = new JsonFileService(_root.Path);

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => source.GetTeamsAsync(Championship.Men, TestCancellation));

        Assert.Contains(Path.Combine("men", "teams", "results.json"), exception.Message, StringComparison.Ordinal);
        Assert.IsAssignableFrom<IOException>(exception.InnerException);
    }

    [Fact]
    public async Task A_file_that_is_not_valid_json_becomes_a_WorldCupDataException()
    {
        Directory.CreateDirectory(_root.Combine("men"));
        await File.WriteAllTextAsync(_root.Combine("men", "matches.json"), "<html>not json</html>", TestCancellation);

        await Assert.ThrowsAsync<WorldCupDataException>(
            () => new JsonFileService(_root.Path).GetMatchesAsync(Championship.Men, TestCancellation));
    }

    private void SaveFixture(string fixture, params string[] target)
    {
        var path = _root.Combine(target);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.Copy(Fixtures.PathOf(fixture), path);
    }
}
