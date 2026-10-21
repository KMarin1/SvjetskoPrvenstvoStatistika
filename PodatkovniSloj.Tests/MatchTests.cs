using PodatkovniSloj.Models;
using static PodatkovniSloj.Tests.Build;

namespace PodatkovniSloj.Tests;

public sealed class MatchTests
{
    private static readonly Match CroatiaNigeria = Match(
        Team("CRO"),
        Team("NGA"),
        "2018-06-16T19:00:00Z",
        homeStatistics: LineUp(["Luka MODRIC"], []),
        awayStatistics: LineUp(["John Obi MIKEL"], []),
        homeEvents: [Event(TypeOfEvent.GoalPenalty, "Luka MODRIC")],
        awayEvents: [Event(TypeOfEvent.RedCard, "John Obi MIKEL")]);

    [Theory]
    [InlineData("CRO", true)]
    [InlineData("nga", true)]
    [InlineData("ARG", false)]
    public void Involves_matches_either_side_ignoring_case(string fifaCode, bool expected) =>
        Assert.Equal(expected, CroatiaNigeria.Involves(fifaCode));

    [Fact]
    public void StatisticsFor_and_EventsFor_pick_the_teams_own_side()
    {
        Assert.Equal("John Obi MIKEL", CroatiaNigeria.StatisticsFor("NGA").StartingEleven[0].Name);
        Assert.Equal(TypeOfEvent.GoalPenalty, Assert.Single(CroatiaNigeria.EventsFor("cro")).TypeOfEvent);
    }

    [Fact]
    public void Asking_for_a_team_that_did_not_play_is_an_error()
    {
        Assert.Throws<ArgumentException>(() => CroatiaNigeria.StatisticsFor("ARG"));
        Assert.Throws<ArgumentException>(() => CroatiaNigeria.EventsFor("ARG"));
    }
}
