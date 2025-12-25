using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Autoskola.DAL.Models
{
    public class VoziloSlika
    {
        public int Id { get; set; }

        [Required]
        public int VoziloId { get; set; }
        public Vozilo Vozilo { get; set; }

        [Required]
        public string PutanjaDoSlike { get; set; }

        public string? Opis { get; set; } 

        public DateTime DatumDodavanja { get; set; } = DateTime.Now;
    }
}
