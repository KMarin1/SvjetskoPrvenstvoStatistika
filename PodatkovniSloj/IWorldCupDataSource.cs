using PodatkovniSloj.Models;

namespace PodatkovniSloj;

/// <summary>Where the client applications get World Cup data from.</summary>
/// <remarks>Every method throws <see cref="WorldCupDataException"/> when the data cannot be loaded.</remarks>
public interface IWorldCupDataSource
{
    /// <summary>Every team's tournament record.</summary>
    Task<IReadOnlyList<TeamResult>> GetTeamsAsync(Championship championship, CancellationToken cancellationToken = default);

    /// <summary>Every match of the tournament.</summary>
    Task<IReadOnlyList<Match>> GetMatchesAsync(Championship championship, CancellationToken cancellationToken = default);

    /// <summary>The matches one team played.</summary>
    Task<IReadOnlyList<Match>> GetTeamMatchesAsync(Championship championship, string fifaCode, CancellationToken cancellationToken = default);
}
