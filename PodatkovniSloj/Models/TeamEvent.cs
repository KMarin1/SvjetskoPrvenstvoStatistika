namespace PodatkovniSloj.Models;

/// <summary>Something that happened to one player during a match: a goal, a card or a substitution.</summary>
public sealed record TeamEvent
{
    public int Id { get; init; }

    public TypeOfEvent TypeOfEvent { get; init; }

    public required string Player { get; init; }

    /// <summary>Match minute as the API writes it, e.g. "90'+2'".</summary>
    public string Time { get; init; } = string.Empty;
}
