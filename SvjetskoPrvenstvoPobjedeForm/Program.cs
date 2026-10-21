using PodatkovniSloj;
using PodatkovniSloj.Config;
using SvjetskoPrvenstvoPobjedeForm.Properties;

namespace SvjetskoPrvenstvoPobjedeForm;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Last line of defence: an error nobody expected is reported and the application keeps running.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => MessageBox.Show(
            $"{Strings.NeocekivanaGreska}{Environment.NewLine}{Environment.NewLine}{e.Exception.Message}",
            Strings.Greska,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        var configService = new ConfigService(ConfigService.DefaultFilePath);
        var settings = LoadSettings(configService);
        UiCulture.Apply(settings.Culture);

        // One HttpClient for the whole run, so connections are pooled and reused.
        using var httpClient = new HttpClient
        {
            BaseAddress = ApiService.DefaultBaseAddress,
            Timeout = ApiService.DefaultTimeout,
        };
        var dataSource = DataSourceFactory.Create(settings, httpClient, AppContext.BaseDirectory);
        Application.Run(new MainForm(settings, configService, dataSource));
    }

    private static UserSettings LoadSettings(ConfigService configService)
    {
        try
        {
            return configService.Load();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(Strings.KrivaPutanjaKonfiguracije);
            return UserSettings.Default;
        }
    }
}
