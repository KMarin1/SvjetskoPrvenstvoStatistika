namespace PodatkovniSloj;

/// <summary>Where the data layer loads World Cup data from.</summary>
public enum DataSourceKind
{
    /// <summary>The public API, see <see cref="ApiService"/>.</summary>
    Api,

    /// <summary>JSON files saved from the API, see <see cref="JsonFileService"/>.</summary>
    JsonFiles,
}
