using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class IspitVozilo
    {
        public int IspitId { get; set; }
        public Ispit Ispit { get; set; }

        public int VoziloId { get; set; }
        public Vozilo Vozilo { get; set; }
    }
}
