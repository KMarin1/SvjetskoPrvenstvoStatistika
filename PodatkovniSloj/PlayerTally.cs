namespace PodatkovniSloj;

/// <summary>A player and how often something happened to them, e.g. goals scored.</summary>
public sealed record PlayerTally(string PlayerName, int Count);
