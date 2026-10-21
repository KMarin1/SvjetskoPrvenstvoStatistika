using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using PodatkovniSloj;
using PodatkovniSloj.Config;

namespace SvjetskoPrvenstvoPobjedeForm
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var configService = new ConfigService(ConfigService.DefaultFilePath);
            var settings = LoadSettings(configService);

            using var httpClient = new HttpClient
            {
                BaseAddress = ApiService.DefaultBaseAddress,
                Timeout = ApiService.DefaultTimeout,
            };
            var dataSource = DataSourceFactory.Create(settings, httpClient, AppContext.BaseDirectory);
            Application.Run(new Form1(settings, configService, dataSource));
        }

        private static UserSettings LoadSettings(ConfigService configService)
        {
            try
            {
                return configService.Load();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show($"Kriva putanja 'config.txt'");
                return UserSettings.Default;
            }
        }
    }
}
