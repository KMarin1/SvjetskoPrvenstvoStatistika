using System.Text.Json;

namespace PodatkovniSloj;

/// <summary>The one set of JSON options every reader of World Cup data uses.</summary>
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
}
