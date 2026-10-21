using PodatkovniSloj.Config;

namespace PodatkovniSloj.Tests;

public sealed class DataSourceFactoryTests
{
    private static readonly string BaseDirectory = Path.Combine(Path.GetTempPath(), "SvjetskoPrvenstvoApp");

    [Fact]
    public void The_default_settings_read_from_the_api()
    {
        using var http = new HttpClient { BaseAddress = ApiService.DefaultBaseAddress };

        var source = DataSourceFactory.Create(UserSettings.Default, http, BaseDirectory);

        Assert.IsType<ApiService>(source);
    }

    [Fact]
    public void Json_files_are_read_from_a_folder_relative_to_the_application()
    {
        using var http = new HttpClient { BaseAddress = ApiService.DefaultBaseAddress };
        var settings = UserSettings.Default with { DataSource = DataSourceKind.JsonFiles, JsonDirectory = "data" };

        var source = Assert.IsType<JsonFileService>(DataSourceFactory.Create(settings, http, BaseDirectory));

        Assert.Equal(Path.Combine(BaseDirectory, "data"), source.RootDirectory);
    }

    [Fact]
    public void An_absolute_json_folder_is_used_as_it_is()
    {
        using var http = new HttpClient { BaseAddress = ApiService.DefaultBaseAddress };
        var elsewhere = Path.Combine(Path.GetTempPath(), "worldcup-json");
        var settings = UserSettings.Default with { DataSource = DataSourceKind.JsonFiles, JsonDirectory = elsewhere };

        var source = Assert.IsType<JsonFileService>(DataSourceFactory.Create(settings, http, BaseDirectory));

        Assert.Equal(elsewhere, source.RootDirectory);
    }
}
