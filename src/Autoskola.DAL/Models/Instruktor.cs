using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.DAL.Models
{
    public class Instruktor
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
        [MaxLength(20)]
        public string? BrojLicence { get; set; }
        [Required]
        [Range(0,100)]
        public int GodineIskustva { get; set; }


        public ICollection<Cas> Casovi { get; set; }
        public ICollection<Ispit> Ispiti { get; set; }

    }
}
