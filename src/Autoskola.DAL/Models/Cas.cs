using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class Cas
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TipCasa TipCasa { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "Broj časa mora biti između 1 i 999")]
        public int BrojCasa { get; set; }  // Obriši [MaxLength(10)] - to je za stringove!

        public DateOnly? Datum { get; set; }

        public int? InstruktorId { get; set; }
        public Instruktor? Instruktor { get; set; }

        public int? VoziloId { get; set; }
        public Vozilo? Vozilo { get; set; }

        public ICollection<KandidatCas> KandidatCasovi { get; set; }
    }

    public enum TipCasa
    {
        Teorijski = 0,
        Praktican = 1
    }
}