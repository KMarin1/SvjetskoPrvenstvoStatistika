using PodatkovniSloj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvjetskoPrvenstvoPobjedeForm
{
    public partial class IgracControl : UserControl
    {
        public Player Player { get; private set; }
        public bool Favorite { get; private set; }
        public IgracControl(Player player, bool favorite)
        {
            InitializeComponent();
            Player = player;
            Favorite = favorite;
            lblIme.Text = $"{player.Name} ({player.ShirtNumber})";
            lblPozicija.Text = player.Position;
            lblKapetan.Visible = player.Captain;
            pbZvjezdica.Visible = favorite;

        }

        public void SetFavorite(bool favorite)
        {
            Favorite = favorite;
            pbZvjezdica.Visible =favorite;
        }

    }
}
