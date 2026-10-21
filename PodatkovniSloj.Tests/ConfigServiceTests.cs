using PodatkovniSloj.Config;

namespace PodatkovniSloj.Tests;

public sealed class ConfigServiceTests : IDisposable
{
    private readonly TempDirectory _folder = new();

    public void Dispose() => _folder.Dispose();

    private string ConfigPath => _folder.Combine("config.txt");

    [Fact]
    public void A_missing_file_is_a_first_run_and_gives_the_defaults()
    {
        var settings = new ConfigService(ConfigPath).Load();

        Assert.Equal(UserSettings.Default, settings);
        Assert.Equal(Championship.Men, settings.Championship);
        Assert.Equal("hr", settings.Culture);
        Assert.Null(settings.Representation);
        Assert.Equal(DataSourceKind.Api, settings.DataSource);
    }

    [Fact]
    public void A_missing_folder_also_gives_the_defaults()
    {
        var settings = new ConfigService(_folder.Combine("not-created-yet", "config.txt")).Load();

        Assert.Equal(UserSettings.Default, settings);
    }

    [Fact]
    public void Every_setting_survives_a_save_and_load()
    {
        var saved = new UserSettings
        {
            Championship = Championship.Women,
            Culture = "en",
            Representation = "CRO",
            DataSource = DataSourceKind.JsonFiles,
            JsonDirectory = "offline",
        };
        var service = new ConfigService(ConfigPath);

        service.Save(saved);

        Assert.Equal(saved, service.Load());
    }

    [Fact]
    public void The_file_is_written_as_key_value_lines()
    {
        new ConfigService(ConfigPath).Save(new UserSettings
        {
            Championship = Championship.Women,
            Culture = "en",
            Representation = "CRO",
        });

        string[] expected =
        [
            "championshiptype=women",
            "culture=en",
            "representation=CRO",
            "datasource=api",
            "jsondirectory=data",
        ];
        Assert.Equal(expected, File.ReadAllLines(ConfigPath));
    }

    [Fact]
    public void A_file_in_the_original_format_still_loads()
    {
        File.WriteAllLines(ConfigPath, ["championshiptype=women", "culture=en", "representation=ENG"]);

        var settings = new ConfigService(ConfigPath).Load();

        Assert.Equal(UserSettings.Default with { Championship = Championship.Women, Culture = "en", Representation = "ENG" }, settings);
    }

    [Fact]
    public void Keys_and_values_are_read_ignoring_case_spaces_blank_lines_and_unknown_keys()
    {
        File.WriteAllLines(ConfigPath, ["", "  ChampionshipType = WOMEN ", "# a note", "CULTURE=En", "representation= cro", "theme=dark"]);

        var settings = new ConfigService(ConfigPath).Load();

        Assert.Equal(UserSettings.Default with { Championship = Championship.Women, Culture = "en", Representation = "CRO" }, settings);
    }

    [Fact]
    public void Invalid_values_fall_back_to_the_defaults()
    {
        File.WriteAllLines(ConfigPath, ["championshiptype=mixed", "culture=de", "representation=", "datasource=ftp", "jsondirectory="]);

        var settings = new ConfigService(ConfigPath).Load();

        Assert.Equal(UserSettings.Default, settings);
    }

    [Fact]
    public void A_value_may_contain_an_equals_sign()
    {
        File.WriteAllLines(ConfigPath, ["datasource=json", @"jsondirectory=D:\data=2018"]);

        var settings = new ConfigService(ConfigPath).Load();

        Assert.Equal(DataSourceKind.JsonFiles, settings.DataSource);
        Assert.Equal(@"D:\data=2018", settings.JsonDirectory);
    }

    [Fact]
    public void Save_creates_the_folder_and_replaces_the_old_file_without_leaving_a_temporary_file()
    {
        var path = _folder.Combine("SvjetskoPrvenstvoStatistika", "config.txt");
        var service = new ConfigService(path);

        service.Save(UserSettings.Default with { Representation = "BRA" });
        service.Save(UserSettings.Default with { Representation = "CRO" });

        Assert.Equal("CRO", service.Load().Representation);
        Assert.Equal(path, Assert.Single(Directory.GetFiles(Path.GetDirectoryName(path)!)));
    }

    [Fact]
    public void A_file_that_cannot_be_read_is_reported_not_replaced_by_defaults()
    {
        File.WriteAllLines(ConfigPath, ["culture=en"]);
        using var exclusiveLock = new FileStream(ConfigPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        Assert.Throws<IOException>(() => new ConfigService(ConfigPath).Load());
    }

    [Fact]
    public void A_folder_that_cannot_be_created_is_reported()
    {
        File.WriteAllText(_folder.Combine("blocker"), "a file where the folder should be");
        var service = new ConfigService(_folder.Combine("blocker", "config.txt"));

        Assert.Throws<IOException>(() => service.Save(UserSettings.Default));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void The_path_must_not_be_blank(string path) =>
        Assert.Throws<ArgumentException>(() => new ConfigService(path));

    [Fact]
    public void The_default_path_is_in_the_users_local_application_data()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        Assert.Equal(Path.Combine(localAppData, "SvjetskoPrvenstvoStatistika", "config.txt"), ConfigService.DefaultFilePath);
    }
}
