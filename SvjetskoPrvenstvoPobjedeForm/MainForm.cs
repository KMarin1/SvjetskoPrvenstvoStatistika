using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using SvjetskoPrvenstvoPobjedeForm.Properties;
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
            ApplyTexts();
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
            UiCulture.Apply(settings.Culture);
            ApplyTexts();
            MessageBox.Show(Strings.PostavkeSpremljene, Strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Sets every visible text from the resources of the current UI culture.</summary>
        private void ApplyTexts()
        {
            Text = Strings.Naslov;
            lblCulture.Text = Strings.Jezik;
            lblChampionshipType.Text = Strings.Prvenstvo;
            lblRepresentation.Text = Strings.Reprezentacija;
            btnSubmit.Text = Strings.Spremi;
        }
    }
}
