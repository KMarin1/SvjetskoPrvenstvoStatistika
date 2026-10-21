namespace PodatkovniSloj.Models;

/// <summary>A team's side of a match: who they are and the final score.</summary>
public sealed record Team
{
    public required string Country { get; init; }

    public required string Code { get; init; }

    public int Goals { get; init; }

    /// <summary>Goals in a penalty shoot-out; 0 when there was none.</summary>
    public int Penalties { get; init; }
}
