namespace PodatkovniSloj;

/// <summary>
/// World Cup data could not be loaded: the source was unreachable, answered with an error,
/// or returned data in a shape the models do not accept.
/// </summary>
public sealed class WorldCupDataException : Exception
{
    public WorldCupDataException()
    {
    }

    public WorldCupDataException(string message)
        : base(message)
    {
    }

    public WorldCupDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
