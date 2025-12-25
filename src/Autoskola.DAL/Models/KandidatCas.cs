using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class KandidatCas
    {
        public int KandidatId { get; set; }
        public Kandidat Kandidat { get; set; }

        public int CasId { get; set; }
        public Cas Cas { get; set; }

        public bool Prisustvovao { get; set; }
        public string? Napomena { get; set; }
    }
}
