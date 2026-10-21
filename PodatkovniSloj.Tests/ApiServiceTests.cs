using System.Globalization;
using System.Net;
using System.Text.Json;
using PodatkovniSloj.Models;

namespace PodatkovniSloj.Tests;

public sealed class ApiServiceTests
{
    private static CancellationToken TestCancellation => TestContext.Current.CancellationToken;

    [Theory]
    [InlineData(Championship.Men, "https://worldcup-vua.nullbit.hr/men/teams/results")]
    [InlineData(Championship.Women, "https://worldcup-vua.nullbit.hr/women/teams/results")]
    public async Task GetTeamsAsync_requests_the_championships_teams_results(Championship championship, string expectedUri)
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.TeamsResults);
        using var http = handler.CreateClient();

        await new ApiService(http).GetTeamsAsync(championship, TestCancellation);

        Assert.Equal(new Uri(expectedUri), Assert.Single(handler.RequestedUris));
    }

    [Fact]
    public async Task GetTeamsAsync_maps_every_field_of_a_team()
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.TeamsResults);
        using var http = handler.CreateClient();

        var teams = await new ApiService(http).GetTeamsAsync(Championship.Men, TestCancellation);

        Assert.Equal(2, teams.Count);
        Assert.Equal(
            new TeamResult
            {
                Id = 13,
                Country = "Croatia",
                AlternateName = "Hrvatska",
                FifaCode = "CRO",
                GroupId = 4,
                GroupLetter = "D",
                Wins = 4,
                Draws = 2,
                Losses = 1,
                GamesPlayed = 7,
                Points = 14,
                GoalsFor = 14,
                GoalsAgainst = 9,
                GoalDifferential = 5,
            },
            teams[1]);
        Assert.Null(teams[0].AlternateName);
        Assert.Equal("Croatia (CRO)", teams[1].DisplayName);
    }

    [Theory]
    [InlineData(Championship.Men, "https://worldcup-vua.nullbit.hr/men/matches")]
    [InlineData(Championship.Women, "https://worldcup-vua.nullbit.hr/women/matches")]
    public async Task GetMatchesAsync_requests_the_championships_matches(Championship championship, string expectedUri)
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        await new ApiService(http).GetMatchesAsync(championship, TestCancellation);

        Assert.Equal(new Uri(expectedUri), Assert.Single(handler.RequestedUris));
    }

    [Fact]
    public async Task GetMatchesAsync_maps_a_match_including_numbers_sent_as_strings()
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        var match = (await new ApiService(http).GetMatchesAsync(Championship.Men, TestCancellation))[0];

        Assert.Equal("Nizhny Novgorod Stadium", match.Location);
        Assert.Equal(300331524, match.FifaId);
        Assert.Equal(43319, match.Attendance);
        Assert.Equal(new DateTimeOffset(2018, 6, 21, 18, 0, 0, TimeSpan.Zero), match.Datetime);
        string[] officials = ["Ravshan IRMATOV", "Abdukhamidullo RASULOV"];
        Assert.Equal(officials, match.Officials);
        Assert.Equal(new Weather { Humidity = 50, TempCelsius = 22, TempFarenheit = 72, WindSpeed = 8, Description = "Sunny" }, match.Weather);
        Assert.Equal(new Team { Country = "Croatia", Code = "CRO", Goals = 3, Penalties = 0 }, match.AwayTeam);
        Assert.Equal("CRO", match.WinnerCode);
        Assert.Null(match.LastScoreUpdateAt);
    }

    [Fact]
    public async Task GetMatchesAsync_maps_line_ups_and_statistics_with_missing_values()
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        var croatia = (await new ApiService(http).GetMatchesAsync(Championship.Men, TestCancellation))[0].AwayTeamStatistics;

        Assert.Equal("4-2-3-1", croatia.Tactics);
        Assert.Equal(42, croatia.BallPossession);
        Assert.Null(croatia.FoulsCommitted);
        Assert.Null(croatia.Clearances);
        Assert.Equal(2, croatia.YellowCards);
        Assert.Equal(new Player { Name = "Danijel SUBASIC", Captain = false, ShirtNumber = 23, Position = Position.Goalie }, croatia.StartingEleven[0]);
        Assert.Equal(Position.Forward, croatia.StartingEleven[1].Position);
        Assert.Equal("Mateo KOVACIC", Assert.Single(croatia.Substitutes).Name);
    }

    [Fact]
    public async Task GetMatchesAsync_reads_every_event_type_the_api_uses()
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        var matches = await new ApiService(http).GetMatchesAsync(Championship.Men, TestCancellation);

        var types = matches.SelectMany(match => match.HomeTeamEvents.Concat(match.AwayTeamEvents)).Select(e => e.TypeOfEvent).ToHashSet();
        Assert.Equal(Enum.GetValues<TypeOfEvent>().ToHashSet(), types);
    }

    [Theory]
    [InlineData("CRO", "https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code=CRO")]
    [InlineData(" cro ", "https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code=cro")]
    [InlineData("A&B", "https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code=A%26B")]
    public async Task GetTeamMatchesAsync_requests_the_country_endpoint_with_the_code_escaped(string fifaCode, string expectedUri)
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        await new ApiService(http).GetTeamMatchesAsync(Championship.Men, fifaCode, TestCancellation);

        Assert.Equal(expectedUri, Assert.Single(handler.RequestedUris).AbsoluteUri);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetTeamMatchesAsync_rejects_a_blank_code_without_calling_the_api(string fifaCode)
    {
        using var handler = FakeHttpMessageHandler.ReturningFixture(Fixtures.Matches);
        using var http = handler.CreateClient();

        await Assert.ThrowsAsync<ArgumentException>(
            () => new ApiService(http).GetTeamMatchesAsync(Championship.Men, fifaCode, TestCancellation));
        Assert.Empty(handler.RequestedUris);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task An_error_status_becomes_a_WorldCupDataException_naming_the_status(HttpStatusCode status)
    {
        using var handler = FakeHttpMessageHandler.Returning("{}", status);
        using var http = handler.CreateClient();

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetTeamsAsync(Championship.Men, TestCancellation));

        Assert.Contains(((int)status).ToString(CultureInfo.InvariantCulture), exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_network_failure_becomes_a_WorldCupDataException_carrying_the_cause()
    {
        var cause = new HttpRequestException("No such host is known.");
        using var handler = FakeHttpMessageHandler.Throwing(cause);
        using var http = handler.CreateClient();

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetMatchesAsync(Championship.Men, TestCancellation));

        Assert.Same(cause, exception.InnerException);
    }

    [Fact]
    public async Task A_timeout_becomes_a_WorldCupDataException()
    {
        using var handler = FakeHttpMessageHandler.Hanging();
        using var http = handler.CreateClient(timeout: TimeSpan.FromMilliseconds(100));

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetTeamsAsync(Championship.Men, TestCancellation));

        Assert.IsType<TaskCanceledException>(exception.InnerException);
    }

    [Fact]
    public async Task Cancellation_by_the_caller_is_not_reported_as_a_data_error()
    {
        using var handler = FakeHttpMessageHandler.Hanging();
        using var http = handler.CreateClient();
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(TestCancellation);
        cancellation.CancelAfter(TimeSpan.FromMilliseconds(50));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => new ApiService(http).GetTeamsAsync(Championship.Men, cancellation.Token));
    }

    [Theory]
    [InlineData("[{")] // malformed
    [InlineData("""[{"country": "Croatia"}]""")] // required fifa_code missing
    [InlineData("""[{"country": null, "fifa_code": "CRO"}]""")] // null in a non-nullable field
    [InlineData("""[{"country": "Croatia", "fifa_code": "CRO", "wins": "four"}]""")] // text where a number belongs
    public async Task Json_the_models_reject_becomes_a_WorldCupDataException_carrying_the_parser_error(string json)
    {
        using var handler = FakeHttpMessageHandler.Returning(json);
        using var http = handler.CreateClient();

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetTeamsAsync(Championship.Men, TestCancellation));

        Assert.IsAssignableFrom<JsonException>(exception.InnerException);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("[null]")]
    public async Task Null_where_teams_are_expected_becomes_a_WorldCupDataException(string json)
    {
        using var handler = FakeHttpMessageHandler.Returning(json);
        using var http = handler.CreateClient();

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetTeamsAsync(Championship.Men, TestCancellation));

        Assert.Contains("null", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task An_unknown_event_type_becomes_a_WorldCupDataException()
    {
        var json = Fixtures.Read(Fixtures.Matches).Replace("\"goal-own\"", "\"var-review\"", StringComparison.Ordinal);
        using var handler = FakeHttpMessageHandler.Returning(json);
        using var http = handler.CreateClient();

        var exception = await Assert.ThrowsAsync<WorldCupDataException>(
            () => new ApiService(http).GetMatchesAsync(Championship.Men, TestCancellation));

        Assert.IsType<JsonException>(exception.InnerException);
    }

    [Fact]
    public void The_HttpClient_must_have_a_base_address()
    {
        using var http = new HttpClient();

        Assert.Throws<ArgumentException>(() => new ApiService(http));
    }
}
