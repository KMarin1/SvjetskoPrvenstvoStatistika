namespace PodatkovniSloj;

/// <summary>The two tournaments the API covers: the 2018 men's and the 2019 women's World Cup.</summary>
public enum Championship
{
    Men,
    Women,
}

/// <summary>Converts <see cref="Championship"/> to and from the name the API and the settings file use.</summary>
public static class ChampionshipExtensions
{
    /// <summary>The API path segment and settings value: "men" or "women".</summary>
    public static string ToApiName(this Championship championship) => championship switch
    {
        Championship.Men => "men",
        Championship.Women => "women",
        _ => throw new ArgumentOutOfRangeException(nameof(championship), championship, "Unknown championship."),
    };

    /// <summary>Parses "men" or "women", ignoring case and surrounding whitespace.</summary>
    public static bool TryParseApiName(string? value, out Championship championship)
    {
        foreach (var candidate in Enum.GetValues<Championship>())
        {
            if (string.Equals(value?.Trim(), candidate.ToApiName(), StringComparison.OrdinalIgnoreCase))
            {
                championship = candidate;
                return true;
            }
        }

        championship = default;
        return false;
    }
}
