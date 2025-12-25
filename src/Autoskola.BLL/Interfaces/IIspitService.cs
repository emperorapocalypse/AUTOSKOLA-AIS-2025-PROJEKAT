using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autoskola.DAL.Models;

namespace Autoskola.BLL.Interfaces
{
    public interface IIspitService
    {
        Task<IEnumerable<Ispit>> GetAllAsync();
        Task<Ispit?> GetByIdAsync(int id);
        Task AddAsync(Ispit ispit);
        Task UpdateAsync(Ispit ispit);
        Task DeleteAsync(int id);
    }
}
