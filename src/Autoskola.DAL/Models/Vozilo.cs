using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class Vozilo
    {
        [Required]
        public int Id {  get; set; }
        [Required]
        [MaxLength(50)]
        public string Marka { get; set; }
        [Required]
        [MaxLength(50)]
        public string Model { get; set; }
        [Required]
        [MaxLength(10)]
        public string Registracija { get; set; }
        public int? GodinaProizvodnje { get; set; }
        [Required]
        public TipGoriva TipGoriva { get; set; }
        [Required]
        public VoznoStanje VoznoStanje { get; set; }


        public ICollection<Cas> Casovi { get; set; }
        public ICollection<IspitVozilo> IspitVozila { get; set; }


        
    }
    public enum TipGoriva
    {
        Benzin = 0,
        Dizel = 1,
        EV = 2
    }

    public enum VoznoStanje
    {
        Vozno = 0,
        NeVozno = 1
    }
}
