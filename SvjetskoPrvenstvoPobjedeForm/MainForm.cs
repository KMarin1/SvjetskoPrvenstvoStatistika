using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using SvjetskoPrvenstvoPobjedeForm.Properties;

namespace SvjetskoPrvenstvoPobjedeForm;

internal sealed partial class MainForm : Form
{
    private readonly ConfigService _configService;
    private readonly IWorldCupDataSource _dataSource;

    // Cancelled when the window closes, so a load still in flight stops instead of touching disposed controls.
    private readonly CancellationTokenSource _closing = new();

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

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _closing.Cancel();
        base.OnFormClosed(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _closing.Dispose();
        }

        base.Dispose(disposing);
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        ddlCulture.DataSource = UserSettings.SupportedCultures.ToList();
        ddlCulture.SelectedItem = _settings.Culture;
        ddlChampionshipType.DataSource = Enum.GetValues<Championship>();
        ddlChampionshipType.SelectedItem = _settings.Championship;

        await LoadTeamsAsync();
    }

    private async void btnSubmit_Click(object? sender, EventArgs e)
    {
        if (ddlChampionshipType.SelectedItem is not Championship championship
            || ddlCulture.SelectedItem is not string culture)
        {
            return;
        }

        var championshipChanged = championship != _settings.Championship;
        var updated = _settings with
        {
            Championship = championship,
            Culture = culture,

            // A team from the other championship's list means nothing in this one.
            Representation = championshipChanged ? null : (cbRepresentation.SelectedItem as TeamResult)?.FifaCode,
        };

        try
        {
            _configService.Save(updated);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ShowError(Strings.GreskaSpremanja, ex);
            return;
        }

        _settings = updated;
        UiCulture.Apply(_settings.Culture);
        ApplyTexts();
        MessageBox.Show(this, Strings.PostavkeSpremljene, Strings.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);

        // A new championship needs its own team list; an empty list means the last load failed.
        if (championshipChanged || cbRepresentation.Items.Count == 0)
        {
            await LoadTeamsAsync();
        }
    }

    /// <summary>
    /// Fills the team list for the saved championship and selects the saved team.
    /// The loading bar shows meanwhile; a failure is reported and leaves the form usable.
    /// </summary>
    private async Task LoadTeamsAsync()
    {
        SetLoading(true);
        try
        {
            var teams = await _dataSource.GetTeamsAsync(_settings.Championship, _closing.Token);
            cbRepresentation.DataSource = teams.OrderBy(team => team.Country, StringComparer.CurrentCulture).ToList();

            var savedTeam = teams.FirstOrDefault(
                team => string.Equals(team.FifaCode, _settings.Representation, StringComparison.OrdinalIgnoreCase));
            if (savedTeam is not null)
            {
                cbRepresentation.SelectedItem = savedTeam;
            }
        }
        catch (OperationCanceledException) when (_closing.IsCancellationRequested)
        {
            // The window is closing; there is nothing left to update.
            return;
        }
        catch (WorldCupDataException ex)
        {
            cbRepresentation.DataSource = null;
            ShowError(Strings.GreskaUcitavanja, ex);
        }

        SetLoading(false);
    }

    private void SetLoading(bool loading)
    {
        pbLoading.Visible = loading;
        cbRepresentation.Enabled = !loading;
        btnSubmit.Enabled = !loading;
    }

    private void ShowError(string message, Exception exception) =>
        MessageBox.Show(
            this,
            $"{message}{Environment.NewLine}{Environment.NewLine}{exception.Message}",
            Strings.Greska,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

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
