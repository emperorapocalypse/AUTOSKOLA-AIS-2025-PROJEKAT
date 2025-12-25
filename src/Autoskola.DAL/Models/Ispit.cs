using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Autoskola.DAL.Models
{
    public class Ispit
    {
        public int Id { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public TimeSpan VremePocetka { get; set; }

        [Required]
        public TipIspita TipIspita { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Bodovi moraju biti između 0 i 100")]
        public int Bodovi { get; set; }

        
        public bool Polozen => Bodovi >= 51;

        [Required]
        public int InstruktorId { get; set; }
        public Instruktor? Instruktor { get; set; }

        public ICollection<KandidatIspit> KandidatIspiti { get; set; } = new List<KandidatIspit>();
        public ICollection<IspitVozilo> IspitVozila { get; set; } = new List<IspitVozilo>();
    }

    public enum TipIspita
    {
        Teorijski = 0,
        Praktican = 1
    }
}