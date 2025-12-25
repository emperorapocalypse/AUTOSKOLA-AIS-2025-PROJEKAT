using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autoskola.DAL.Models;

namespace Autoskola.BLL.Interfaces
{
    public interface IInstruktorService
    {
        Task<IEnumerable<Instruktor>> GetAllAsync();
        Task<Instruktor?> GetByIdAsync(int id);
        Task AddAsync(Instruktor instruktor);
        Task UpdateAsync(Instruktor instruktor);
        Task DeleteAsync(int id);
    }
}
