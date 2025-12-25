using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class Kandidat
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }
        [Required]
        [MaxLength(50)]
        public string Prezime { get; set; }
        [Required]
        [MaxLength(13)]
        public string JMBG { get; set; }
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        [Required]
        public DateTime DatumUpisa { get; set; }
        public string? ProfilnaSlika { get; set; }

        public ICollection<KandidatCas> KandidatCasovi { get; set; }
        public ICollection<KandidatIspit> KandidatIspiti { get; set; }

    }
}
