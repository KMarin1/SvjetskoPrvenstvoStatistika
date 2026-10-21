using System;
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

            using var httpClient = new HttpClient
            {
                BaseAddress = ApiService.DefaultBaseAddress,
                Timeout = ApiService.DefaultTimeout,
            };
            Application.Run(new Form1(new ApiService(httpClient), new ConfigService(ConfigService.DefaultFilePath)));
        }
    }
}
