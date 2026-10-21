using PodatkovniSloj.Models;

namespace PodatkovniSloj;

/// <summary>Reads World Cup data from the public API at worldcup-vua.nullbit.hr.</summary>
public sealed class ApiService : IWorldCupDataSource
{
    /// <summary>The API root. The plain-http address redirects here.</summary>
    public static Uri DefaultBaseAddress { get; } = new("https://worldcup-vua.nullbit.hr/");

    /// <summary>How long one request may take, download included.</summary>
    public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(30);

    private readonly HttpClient _httpClient;

    /// <param name="httpClient">
    /// A long-lived client with <see cref="HttpClient.BaseAddress"/> set. The caller owns it;
    /// one instance is shared for the whole application so connections are reused.
    /// </param>
    public ApiService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        if (httpClient.BaseAddress is null)
        {
            throw new ArgumentException("The HttpClient needs a BaseAddress.", nameof(httpClient));
        }

        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<TeamResult>> GetTeamsAsync(Championship championship, CancellationToken cancellationToken = default) =>
        GetListAsync<TeamResult>($"{championship.ToApiName()}/teams/results", cancellationToken);

    public Task<IReadOnlyList<Match>> GetMatchesAsync(Championship championship, CancellationToken cancellationToken = default) =>
        GetListAsync<Match>($"{championship.ToApiName()}/matches", cancellationToken);

    public Task<IReadOnlyList<Match>> GetTeamMatchesAsync(Championship championship, string fifaCode, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fifaCode);
        return GetListAsync<Match>(
            $"{championship.ToApiName()}/matches/country?fifa_code={Uri.EscapeDataString(fifaCode.Trim())}",
            cancellationToken);
    }

    private async Task<IReadOnlyList<T>> GetListAsync<T>(string path, CancellationToken cancellationToken)
        where T : class
    {
        var source = $"The World Cup API (/{path})";
        try
        {
            // The default completion option buffers the body, so the timeout covers the download too.
            using var response = await _httpClient.GetAsync(new Uri(path, UriKind.Relative), cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new WorldCupDataException($"{source} answered {(int)response.StatusCode} {response.ReasonPhrase}.");
            }

            var body = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await WorldCupJson.ReadListAsync<T>(body, source, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new WorldCupDataException($"{source} could not be reached: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new WorldCupDataException($"{source} did not answer within {_httpClient.Timeout.TotalSeconds:0} seconds.", ex);
        }
    }
}
