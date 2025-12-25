using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autoskola.DAL.Models;

namespace Autoskola.BLL.Interfaces
{
    public interface IVoziloService
    {
        Task<IEnumerable<Vozilo>> GetAllAsync();
        Task<Vozilo?> GetByIdAsync(int id);
        Task AddAsync(Vozilo vozilo);
        Task UpdateAsync(Vozilo vozilo);
        Task DeleteAsync(int id);
    }
}
