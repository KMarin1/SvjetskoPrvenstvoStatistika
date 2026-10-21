namespace PodatkovniSloj.Models;

/// <summary>One team's numbers and line-up for one match.</summary>
public sealed record TeamStatistics
{
    public string Country { get; init; } = string.Empty;

    public int AttemptsOnGoal { get; init; }

    public int OnTarget { get; init; }

    public int OffTarget { get; init; }

    public int Blocked { get; init; }

    public int Woodwork { get; init; }

    public int Corners { get; init; }

    public int Offsides { get; init; }

    public int BallPossession { get; init; }

    public int PassAccuracy { get; init; }

    public int NumPasses { get; init; }

    public int PassesCompleted { get; init; }

    public int DistanceCovered { get; init; }

    public int BallsRecovered { get; init; }

    public int Tackles { get; init; }

    /// <summary>Missing (null) for some matches in the source data.</summary>
    public int? Clearances { get; init; }

    /// <summary>Missing (null) for some matches in the source data.</summary>
    public int? YellowCards { get; init; }

    public int RedCards { get; init; }

    /// <summary>Missing (null) for some matches in the source data.</summary>
    public int? FoulsCommitted { get; init; }

    public string Tactics { get; init; } = string.Empty;

    public IReadOnlyList<Player> StartingEleven { get; init; } = [];

    public IReadOnlyList<Player> Substitutes { get; init; } = [];
}
