namespace PodatkovniSloj.Config;

/// <summary>
/// Keeps <see cref="UserSettings"/> in a text file of key=value lines, the format the
/// original config.txt used, so an existing file still loads.
/// </summary>
public sealed class ConfigService
{
    private const string ChampionshipKey = "championshiptype";
    private const string CultureKey = "culture";
    private const string RepresentationKey = "representation";

    /// <param name="filePath">The settings file. Its folder is created on the first save.</param>
    public ConfigService(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        FilePath = Path.GetFullPath(filePath);
    }

    /// <summary>
    /// <c>%LOCALAPPDATA%\SvjetskoPrvenstvoStatistika\config.txt</c>: per user, writable without
    /// admin rights, and not overwritten when the application is rebuilt or reinstalled.
    /// </summary>
    public static string DefaultFilePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SvjetskoPrvenstvoStatistika",
        "config.txt");

    public string FilePath { get; }

    /// <summary>
    /// Reads the settings. A missing file is a first run and gives <see cref="UserSettings.Default"/>;
    /// unknown keys and invalid values fall back to the defaults instead of failing.
    /// </summary>
    /// <exception cref="IOException">The file exists but cannot be read.</exception>
    /// <exception cref="UnauthorizedAccessException">The file exists but access is denied.</exception>
    public UserSettings Load()
    {
        string[] lines;
        try
        {
            lines = File.ReadAllLines(FilePath);
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return UserSettings.Default;
        }

        return Parse(lines);
    }

    /// <summary>
    /// Writes the settings. The new content goes to a temporary file that then replaces the old
    /// one, so an interrupted save leaves the previous settings intact instead of a partial file.
    /// </summary>
    /// <exception cref="IOException">The file or its folder cannot be written.</exception>
    /// <exception cref="UnauthorizedAccessException">Access to the file or its folder is denied.</exception>
    public void Save(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var temporaryPath = FilePath + ".tmp";
        File.WriteAllLines(temporaryPath, Format(settings));
        File.Move(temporaryPath, FilePath, overwrite: true);
    }

    private static UserSettings Parse(IEnumerable<string> lines)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines)
        {
            var separator = line.IndexOf('=');
            if (separator > 0)
            {
                values[line[..separator].Trim()] = line[(separator + 1)..].Trim();
            }
        }

        var defaults = UserSettings.Default;
        var culture = UserSettings.SupportedCultures.FirstOrDefault(
            supported => string.Equals(supported, values.GetValueOrDefault(CultureKey), StringComparison.OrdinalIgnoreCase));
        var representation = values.GetValueOrDefault(RepresentationKey);

        return new UserSettings
        {
            Championship = ChampionshipExtensions.TryParseApiName(values.GetValueOrDefault(ChampionshipKey), out var championship)
                ? championship
                : defaults.Championship,
            Culture = culture ?? defaults.Culture,
            Representation = string.IsNullOrEmpty(representation) ? null : representation.ToUpperInvariant(),
        };
    }

    private static IEnumerable<string> Format(UserSettings settings)
    {
        yield return $"{ChampionshipKey}={settings.Championship.ToApiName()}";
        yield return $"{CultureKey}={settings.Culture}";
        if (settings.Representation is not null)
        {
            yield return $"{RepresentationKey}={settings.Representation}";
        }
    }
}
