using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class KandidatIspit
    {
        public int KandidatId { get; set; }
        public Kandidat Kandidat { get; set; }
        
        public int IspitId { get; set; }
        public Ispit Ispit { get; set; }

        public bool Polozio { get; set; }
        public int BrojBodova { get; set; }
        public string? Napomena { get; set; }

    }
}
