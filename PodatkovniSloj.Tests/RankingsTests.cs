using PodatkovniSloj.Models;
using static PodatkovniSloj.Tests.Build;

namespace PodatkovniSloj.Tests;

public sealed class RankingsTests
{
    // Croatia at home to Nigeria, then away at Argentina; plus a match Croatia did not play.
    private static readonly Match[] Matches =
    [
        Match(
            Team("CRO", "Croatia"),
            Team("NGA", "Nigeria"),
            "2018-06-16T19:00:00Z",
            homeEvents:
            [
                Event(TypeOfEvent.GoalPenalty, "Luka MODRIC"),
                Event(TypeOfEvent.YellowCard, "Ante REBIC"),
                Event(TypeOfEvent.SubstitutionOut, "Ante REBIC"),
                Event(TypeOfEvent.YellowCard, "Sime VRSALJKO"),
            ],
            awayEvents:
            [
                Event(TypeOfEvent.GoalOwn, "Oghenekaro ETEBO"),
                Event(TypeOfEvent.YellowCard, "William TROOST-EKONG"),
            ],
            attendance: 31136,
            location: "Kaliningrad Stadium"),
        Match(
            Team("ARG", "Argentina"),
            Team("CRO", "Croatia"),
            "2018-06-21T18:00:00Z",
            homeEvents: [Event(TypeOfEvent.Goal, "Lionel MESSI"), Event(TypeOfEvent.YellowCard, "Marcos ACUNA")],
            awayEvents:
            [
                Event(TypeOfEvent.Goal, "Ante REBIC"),
                Event(TypeOfEvent.Goal, "Luka MODRIC"),
                Event(TypeOfEvent.Goal, "Ivan RAKITIC"),
                Event(TypeOfEvent.GoalOwn, "Dejan LOVREN"),
                Event(TypeOfEvent.YellowCard, "Ante REBIC"),
                Event(TypeOfEvent.YellowCard, "Mario MANDZUKIC"),
                Event(TypeOfEvent.YellowCardSecond, "Mario MANDZUKIC"),
                Event(TypeOfEvent.RedCard, "Mario MANDZUKIC"),
            ],
            attendance: 43319,
            location: "Nizhny Novgorod Stadium"),
        Match(
            Team("RUS", "Russia"),
            Team("KSA", "Saudi Arabia"),
            "2018-06-14T15:00:00Z",
            homeEvents: [Event(TypeOfEvent.Goal, "Denis CHERYSHEV")],
            attendance: 78011,
            location: "Luzhniki Stadium"),
    ];

    [Fact]
    public void Goals_count_goals_and_penalties_most_first_then_by_name()
    {
        PlayerTally[] expected =
        [
            new("Luka MODRIC", 2),
            new("Ante REBIC", 1),
            new("Ivan RAKITIC", 1),
        ];

        Assert.Equal(expected, Rankings.Goals(Matches, "CRO"));
    }

    [Fact]
    public void Goals_leave_out_own_goals()
    {
        Assert.DoesNotContain(Rankings.Goals(Matches, "CRO"), tally => tally.PlayerName == "Dejan LOVREN");
        Assert.Empty(Rankings.Goals(Matches, "NGA"));
    }

    [Fact]
    public void Goals_count_only_the_chosen_teams_players()
    {
        Assert.Equal([new PlayerTally("Lionel MESSI", 1)], Rankings.Goals(Matches, "arg"));
    }

    [Fact]
    public void Yellow_cards_count_a_second_yellow_as_a_yellow_card()
    {
        PlayerTally[] expected =
        [
            new("Ante REBIC", 2),
            new("Mario MANDZUKIC", 2),
            new("Sime VRSALJKO", 1),
        ];

        Assert.Equal(expected, Rankings.YellowCards(Matches, "CRO"));
    }

    [Fact]
    public void Attendance_lists_the_teams_matches_largest_crowd_first()
    {
        AttendanceEntry[] expected =
        [
            new("Nizhny Novgorod Stadium", 43319, "Argentina", "Croatia"),
            new("Kaliningrad Stadium", 31136, "Croatia", "Nigeria"),
        ];

        Assert.Equal(expected, Rankings.Attendance(Matches, "CRO"));
    }

    [Fact]
    public void Attendance_puts_equal_crowds_in_kick_off_order()
    {
        Match[] matches =
        [
            Match(Team("FRA"), Team("CRO"), "2018-07-15T15:00:00Z", attendance: 78011, location: "Final"),
            Match(Team("CRO"), Team("ENG"), "2018-07-11T18:00:00Z", attendance: 78011, location: "Semi-final"),
        ];

        Assert.Equal(["Semi-final", "Final"], Rankings.Attendance(matches, "CRO").Select(entry => entry.Location));
    }

    [Fact]
    public void Every_ranking_is_empty_for_a_team_that_did_not_play()
    {
        Assert.Empty(Rankings.Goals(Matches, "BRA"));
        Assert.Empty(Rankings.YellowCards(Matches, "BRA"));
        Assert.Empty(Rankings.Attendance(Matches, "BRA"));
    }

    [Fact]
    public void Every_ranking_rejects_missing_input()
    {
        Assert.Throws<ArgumentNullException>(() => Rankings.Goals(null!, "CRO"));
        Assert.Throws<ArgumentNullException>(() => Rankings.YellowCards(null!, "CRO"));
        Assert.Throws<ArgumentNullException>(() => Rankings.Attendance(null!, "CRO"));
        Assert.Throws<ArgumentException>(() => Rankings.Goals(Matches, " "));
        Assert.Throws<ArgumentException>(() => Rankings.YellowCards(Matches, ""));
        Assert.Throws<ArgumentException>(() => Rankings.Attendance(Matches, " "));
    }
}
