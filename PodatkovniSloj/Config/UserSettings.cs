namespace PodatkovniSloj.Config;

/// <summary>The choices the user makes once and keeps between runs.</summary>
public sealed record UserSettings
{
    /// <summary>The UI languages the application offers: Croatian and English.</summary>
    public static IReadOnlyList<string> SupportedCultures { get; } = ["hr", "en"];

    /// <summary>What a first run starts with.</summary>
    public static UserSettings Default { get; } = new();

    public Championship Championship { get; init; } = Championship.Men;

    /// <summary>One of <see cref="SupportedCultures"/>.</summary>
    public string Culture { get; init; } = "hr";

    /// <summary>FIFA code of the chosen national team, or null before one is chosen.</summary>
    public string? Representation { get; init; }
}
