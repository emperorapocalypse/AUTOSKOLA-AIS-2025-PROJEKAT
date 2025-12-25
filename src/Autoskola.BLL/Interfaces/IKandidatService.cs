using Autoskola.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoskola.BLL.Interfaces
{
    public interface IKandidatService
    {
        Task<IEnumerable<Kandidat>> GetAllAsync();
        Task<Kandidat?> GetByIdAsync(int id);
        Task AddAsync(Kandidat kandidat);
        Task UpdateAsync(Kandidat kandidat);
        Task DeleteAsync(int id);
    }
}
