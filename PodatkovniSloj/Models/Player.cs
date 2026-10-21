namespace PodatkovniSloj.Models;

/// <summary>A player in a match line-up (<c>starting_eleven</c> or <c>substitutes</c>).</summary>
public sealed record Player
{
    public required string Name { get; init; }

    public bool Captain { get; init; }

    public int ShirtNumber { get; init; }

    public Position Position { get; init; }
}
