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
        Task AddAsync(Cas cas);
        Task UpdateAsync(Cas cas);
        Task DeleteAsync(int id);
    }
}
