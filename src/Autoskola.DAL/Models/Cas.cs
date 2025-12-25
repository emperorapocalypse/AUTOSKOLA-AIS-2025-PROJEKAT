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
        [MaxLength(10)]
        public int BrojCasa { get; set; }
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
