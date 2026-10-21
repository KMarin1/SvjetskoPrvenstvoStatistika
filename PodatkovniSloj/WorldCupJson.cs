using System.Text.Json;

namespace PodatkovniSloj;

/// <summary>The one place World Cup JSON is turned into models.</summary>
internal static class WorldCupJson
{
    /// <summary>
    /// The API uses snake_case names. Nullable annotations are enforced, so a null where the
    /// model does not allow one is a format error instead of a null that surfaces later.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        RespectNullableAnnotations = true,
    };

    /// <summary>Reads a JSON array of <typeparamref name="T"/>.</summary>
    /// <param name="json">The JSON text.</param>
    /// <param name="source">Where the JSON came from, for the error message.</param>
    /// <param name="cancellationToken">Cancels the read.</param>
    /// <exception cref="WorldCupDataException">The JSON is malformed, null, or does not fit the model.</exception>
    public static async Task<IReadOnlyList<T>> ReadListAsync<T>(Stream json, string source, CancellationToken cancellationToken)
        where T : class
    {
        List<T>? items;
        try
        {
            items = await JsonSerializer.DeserializeAsync<List<T>>(json, Options, cancellationToken).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            throw new WorldCupDataException($"{source} is not in the expected format: {ex.Message}", ex);
        }

        // Nullable annotations are not enforced on list elements, so [null] has to be caught here.
        if (items is null || items.Exists(item => item is null))
        {
            throw new WorldCupDataException($"{source} contains null where a list of items was expected.");
        }

        return items;
    }
}
