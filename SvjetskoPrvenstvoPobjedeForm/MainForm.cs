using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SvjetskoPrvenstvoPobjedeForm
{
    public partial class MainForm : Form
    {
        private readonly ConfigService configService;
        private UserSettings settings;
        private readonly IWorldCupDataSource apiService;

        public MainForm(UserSettings settings, ConfigService configService, IWorldCupDataSource apiService)
        {
            InitializeComponent();
            this.settings = settings;
            this.apiService = apiService;
            this.configService = configService;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            ddlCulture.DataSource = UserSettings.SupportedCultures.ToList();
            ddlChampionshipType.DataSource = Enum.GetValues<Championship>().Select(c => c.ToApiName()).ToList();

            var teams = await apiService.GetTeamsAsync(settings.Championship);
            cbRepresentation.DataSource = teams;

            var team = teams.FirstOrDefault(t => string.Equals(t.FifaCode, settings.Representation, StringComparison.OrdinalIgnoreCase));
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
