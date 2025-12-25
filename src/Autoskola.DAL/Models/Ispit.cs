using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class Ispit
    {
        public int Id { get; set; }
        [Required]
        public DateTime Datum {  get; set; }
        [Required]
        public TimeSpan VremePocetka {  get; set; }
        [Required]
        public TipIspita TipIspita { get; set; }
        [Required]
        public int InstruktorId { get; set; }
        public Instruktor Instruktor { get; set; }

        public ICollection<KandidatIspit> KandidatIspiti { get; set; }
        public ICollection<IspitVozilo> IspitVozila { get; set; }




    }

    public enum TipIspita
    {
        Teorijski = 0,
        Praktican = 1
    }


}
