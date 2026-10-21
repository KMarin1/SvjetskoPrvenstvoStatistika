using static PodatkovniSloj.Tests.Build;

namespace PodatkovniSloj.Tests;

public sealed class SquadTests
{
    private static readonly Models.Match Opener = Match(
        Team("CRO"),
        Team("NGA"),
        "2018-06-16T19:00:00Z",
        homeStatistics: LineUp(["Danijel SUBASIC", "Luka MODRIC"], ["Mateo KOVACIC"]),
        awayStatistics: LineUp(["Francis UZOHO"], ["Odion IGHALO"]));

    private static readonly Models.Match SecondMatch = Match(
        Team("ARG"),
        Team("CRO"),
        "2018-06-21T18:00:00Z",
        awayStatistics: LineUp(["Danijel SUBASIC", "Ivan PERISIC"], ["Mateo KOVACIC"]));

    [Fact]
    public void Uses_the_earliest_match_even_when_it_is_not_listed_first()
    {
        var squad = Squad.FromFirstMatch([SecondMatch, Opener], "CRO");

        Assert.Equal(["Danijel SUBASIC", "Luka MODRIC", "Mateo KOVACIC"], squad.Select(player => player.Name));
    }

    [Fact]
    public void Lists_the_starting_eleven_before_the_substitutes()
    {
        var squad = Squad.FromFirstMatch([Opener], "NGA");

        Assert.Equal(["Francis UZOHO", "Odion IGHALO"], squad.Select(player => player.Name));
    }

    [Fact]
    public void Takes_the_away_side_when_the_team_played_away()
    {
        var squad = Squad.FromFirstMatch([SecondMatch], "CRO");

        Assert.Contains(squad, player => player.Name == "Ivan PERISIC");
    }

    [Fact]
    public void Ignores_the_case_of_the_fifa_code()
    {
        Assert.Equal(3, Squad.FromFirstMatch([Opener], "cro").Count);
    }

    [Fact]
    public void Lists_a_player_named_in_both_groups_once()
    {
        var match = Match(Team("CRO"), Team("NGA"), "2018-06-16T19:00:00Z", homeStatistics: LineUp(["Luka MODRIC"], ["Luka MODRIC", "Mateo KOVACIC"]));

        Assert.Equal(["Luka MODRIC", "Mateo KOVACIC"], Squad.FromFirstMatch([match], "CRO").Select(player => player.Name));
    }

    [Fact]
    public void Is_empty_for_a_team_without_a_match()
    {
        Assert.Empty(Squad.FromFirstMatch([Opener, SecondMatch], "BRA"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Rejects_a_blank_fifa_code(string fifaCode) =>
        Assert.Throws<ArgumentException>(() => Squad.FromFirstMatch([Opener], fifaCode));
}
