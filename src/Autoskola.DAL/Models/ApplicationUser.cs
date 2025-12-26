using Microsoft.AspNetCore.Identity;

namespace Autoskola.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? ProfilnaSlika { get; set; }

        // Za kandidate - povezivanje sa Kandidat tabelom
        public int? KandidatId { get; set; }
        public Kandidat? Kandidat { get; set; }
    }
}