using PodatkovniSloj.Models;

namespace PodatkovniSloj;

/// <summary>
/// Reads World Cup data from JSON files saved from the API, for working offline. The layout
/// mirrors the API paths: <c>{root}/men/teams/results.json</c> and <c>{root}/men/matches.json</c>,
/// and the same under <c>women</c>.
/// </summary>
public sealed class JsonFileService : IWorldCupDataSource
{
    /// <param name="rootDirectory">The folder that holds the <c>men</c> and <c>women</c> folders.</param>
    public JsonFileService(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        RootDirectory = Path.GetFullPath(rootDirectory);
    }

    public string RootDirectory { get; }

    public Task<IReadOnlyList<TeamResult>> GetTeamsAsync(Championship championship, CancellationToken cancellationToken = default) =>
        ReadListAsync<TeamResult>(Path.Combine(RootDirectory, championship.ToApiName(), "teams", "results.json"), cancellationToken);

    public Task<IReadOnlyList<Match>> GetMatchesAsync(Championship championship, CancellationToken cancellationToken = default) =>
        ReadListAsync<Match>(Path.Combine(RootDirectory, championship.ToApiName(), "matches.json"), cancellationToken);

    /// <summary>The API filters on the server; here the full match file is read and filtered.</summary>
    public async Task<IReadOnlyList<Match>> GetTeamMatchesAsync(Championship championship, string fifaCode, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fifaCode);
        var matches = await GetMatchesAsync(championship, cancellationToken).ConfigureAwait(false);
        return [.. matches.Where(match => match.Involves(fifaCode.Trim()))];
    }

    private static async Task<IReadOnlyList<T>> ReadListAsync<T>(string path, CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, FileOptions.Asynchronous);
            await using (file.ConfigureAwait(false))
            {
                return await WorldCupJson.ReadListAsync<T>(file, path, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new WorldCupDataException($"Could not read {path}.", ex);
        }
    }
}
