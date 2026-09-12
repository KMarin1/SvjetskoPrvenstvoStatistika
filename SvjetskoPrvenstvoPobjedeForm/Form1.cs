using PodatkovniSloj;
using PodatkovniSloj.Config;
using PodatkovniSloj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
namespace SvjetskoPrvenstvoPobjedeForm
{
    public partial class Form1 : Form
    {
        string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Config", "config.txt");
        private readonly ConfigService configService = new ConfigService();
        private Dictionary<string, string> config;
        private readonly ApiService apiService = new ApiService();
        public Form1()
        {
            InitializeComponent();
            
        }
        private async void Form1_Load(object sender, EventArgs e)
        {

            var allowedCultures = new[] { "hr", "en" };
            var allowedChampionShipTypes = new[] { "men", "women" };
            ddlCulture.DataSource = allowedCultures.ToList();
            ddlChampionshipType.DataSource = allowedChampionShipTypes.ToList();


            try
            {
                config = configService.LoadConfig(configPath);
            }
            catch (Exception)
            {
                MessageBox.Show($"Kriva putanja 'config.txt'");
                config = new Dictionary<string, string>();
            }

            string championship = config.ContainsKey("championshiptype") ? config["championshiptype"].ToLower() : null;
            string culture = config.ContainsKey("culture") ? config["culture"].ToLower() : null;

            var teams = await apiService.GetTeamsAsync(championship);
            var matches = await apiService.GetMatchesAsync(championship);
            var repCode = config["representation"];
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

            if (config.ContainsKey("representation"))
            {
                string fifacode = config["representation"];
                var team = teams.FirstOrDefault(t => t.FifaCode.Equals(fifacode, StringComparison.OrdinalIgnoreCase));
                if(team != null)
                    cbRepresentation.SelectedItem = team;
            }

            if (championship != null && allowedChampionShipTypes.Contains(championship))
            {
                ddlChampionshipType.SelectedItem = championship.ToLower(); 
            }
            if (culture != null && allowedCultures.Contains(culture))
            {
                ddlCulture.SelectedItem = culture; 
            }


        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            config["championshiptype"] = ddlChampionshipType.SelectedItem?.ToString();
            config["culture"] = (ddlCulture.SelectedItem).ToString();
            config["representation"] = (cbRepresentation.SelectedItem)?.ToString();
            configService.SaveConfig(config, configPath);            
            MessageBox.Show("Postavke su spremljene.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
