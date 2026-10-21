using PodatkovniSloj.Config;

namespace PodatkovniSloj;

/// <summary>Picks the data source the settings ask for, so the client applications never branch on it.</summary>
public static class DataSourceFactory
{
    /// <param name="settings">Supplies <see cref="UserSettings.DataSource"/> and <see cref="UserSettings.JsonDirectory"/>.</param>
    /// <param name="httpClient">The application's shared client, used when the source is the API.</param>
    /// <param name="baseDirectory">What a relative <see cref="UserSettings.JsonDirectory"/> is resolved against, normally the application folder.</param>
    public static IWorldCupDataSource Create(UserSettings settings, HttpClient httpClient, string baseDirectory)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);

        return settings.DataSource switch
        {
            DataSourceKind.Api => new ApiService(httpClient),
            DataSourceKind.JsonFiles => new JsonFileService(Path.Combine(baseDirectory, settings.JsonDirectory)),
            _ => throw new ArgumentOutOfRangeException(nameof(settings), settings.DataSource, "Unknown data source."),
        };
    }
}
