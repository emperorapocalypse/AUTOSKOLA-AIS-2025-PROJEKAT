using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autoskola.DAL.Models;

namespace Autoskola.BLL.Interfaces
{
    public interface ICasService
    {
        Task<IEnumerable<Cas>> GetAllAsync();
        Task<Cas?> GetByIdAsync(int id);
        Task AddAsync(Cas cas, List<int> kandidatIds);
        Task UpdateAsync(Cas cas, List<(int KandidatId, bool Prisustvovao, string? Napomena)> kandidatCasovi);
        Task DeleteAsync(int id);

        // Metode za dropdown liste
        Task<IEnumerable<Instruktor>> GetAllInstruktoriAsync();
        Task<IEnumerable<Kandidat>> GetAllKandidatiAsync();
        Task<IEnumerable<Vozilo>> GetAllVozilaAsync();
    }
}