
using Autoskola.DAL.Models;

namespace Autoskola.BLL.Interfaces
{
    public interface IIspitService
    {
        Task<IEnumerable<Ispit>> GetAllAsync();
        Task<Ispit?> GetByIdAsync(int id);
        Task AddAsync(Ispit ispit, int kandidatId, List<int>? vozilaIds);
        Task UpdateAsync(Ispit ispit, int kandidatId, List<int>? vozilaIds);
        Task DeleteAsync(int id);
        Task<IEnumerable<Instruktor>> GetAllInstruktoriAsync();
        Task<IEnumerable<Kandidat>> GetAllKandidatiAsync();
        Task<IEnumerable<Vozilo>> GetAllVozilaAsync();
    }
}
