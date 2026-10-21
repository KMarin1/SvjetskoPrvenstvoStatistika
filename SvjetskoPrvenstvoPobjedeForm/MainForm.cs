using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using SvjetskoPrvenstvoPobjedeForm.Properties;

namespace SvjetskoPrvenstvoPobjedeForm;

internal sealed partial class MainForm : Form
{
    private readonly ConfigService _configService;
    private readonly IWorldCupDataSource _dataSource;
    private UserSettings _settings;

    public MainForm(UserSettings settings, ConfigService configService, IWorldCupDataSource dataSource)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(configService);
        ArgumentNullException.ThrowIfNull(dataSource);

        InitializeComponent();
        _settings = settings;
        _configService = configService;
        _dataSource = dataSource;
        ddlChampionshipType.Format += (_, e) =>
        {
            if (e.ListItem is Championship championship)
            {
                e.Value = championship.ToApiName();
            }
        };
        ApplyTexts();
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        ddlCulture.DataSource = UserSettings.SupportedCultures.ToList();
        ddlCulture.SelectedItem = _settings.Culture;
        ddlChampionshipType.DataSource = Enum.GetValues<Championship>();
        ddlChampionshipType.SelectedItem = _settings.Championship;

        var teams = await _dataSource.GetTeamsAsync(_settings.Championship);
        cbRepresentation.DataSource = teams.ToList();

        var savedTeam = teams.FirstOrDefault(
            team => string.Equals(team.FifaCode, _settings.Representation, StringComparison.OrdinalIgnoreCase));
        if (savedTeam is not null)
        {
            cbRepresentation.SelectedItem = savedTeam;
        }
    }

    private void btnSubmit_Click(object? sender, EventArgs e)
    {
        if (ddlChampionshipType.SelectedItem is not Championship championship
            || ddlCulture.SelectedItem is not string culture)
        {
            return;
        }

        _settings = _settings with
        {
            Championship = championship,
            Culture = culture,
            Representation = (cbRepresentation.SelectedItem as TeamResult)?.FifaCode,
        };
        _configService.Save(_settings);
        UiCulture.Apply(_settings.Culture);
        ApplyTexts();
        MessageBox.Show(this, Strings.PostavkeSpremljene, Strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
