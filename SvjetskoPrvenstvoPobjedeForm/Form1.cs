using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace SvjetskoPrvenstvoPobjedeForm
{
    public partial class Form1 : Form
    {
        private readonly ConfigService configService;
        private UserSettings settings = UserSettings.Default;
        private readonly IWorldCupDataSource apiService;
        public Form1(IWorldCupDataSource apiService, ConfigService configService)
        {
            InitializeComponent();
            this.apiService = apiService;
            this.configService = configService;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            ddlCulture.DataSource = UserSettings.SupportedCultures.ToList();
            ddlChampionshipType.DataSource = Enum.GetValues<Championship>().Select(c => c.ToApiName()).ToList();

            try
            {
                settings = configService.Load();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show($"Kriva putanja 'config.txt'");
            }

            var teams = await apiService.GetTeamsAsync(settings.Championship);
            var matches = await apiService.GetMatchesAsync(settings.Championship);
            var repCode = settings.Representation;
            var firstMatch = matches.FirstOrDefault(m => m.HomeTeam.Code == repCode || m.AwayTeam.Code == repCode);
            var players = new List<Player>();

            if(firstMatch != null)
            {
                var teamStats = firstMatch.HomeTeam.Code == repCode ? firstMatch.HomeTeamStatistics : firstMatch.AwayTeamStatistics;
                players.AddRange(teamStats.StartingEleven);
                players.AddRange(teamStats.Substitutes);
            }
            //
            cbRepresentation.DataSource = teams;
            //

            var team = teams.FirstOrDefault(t => string.Equals(t.FifaCode, repCode, StringComparison.OrdinalIgnoreCase));
            if (team != null)
                cbRepresentation.SelectedItem = team;

            ddlChampionshipType.SelectedItem = settings.Championship.ToApiName();
            ddlCulture.SelectedItem = settings.Culture;
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            ChampionshipExtensions.TryParseApiName(ddlChampionshipType.SelectedItem as string, out var championship);
            settings = settings with
            {
                Championship = championship,
                Culture = (string)ddlCulture.SelectedItem,
                Representation = (cbRepresentation.SelectedItem as TeamResult)?.FifaCode,
            };
            configService.Save(settings);
            MessageBox.Show("Postavke su spremljene.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
