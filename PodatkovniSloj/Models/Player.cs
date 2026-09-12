using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodatkovniSloj.Models
{
    public class Player
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int ShirtNumber { get; set; }
        public bool Captain { get; set; }
        public bool Favorite { get; set; }
    }
}
