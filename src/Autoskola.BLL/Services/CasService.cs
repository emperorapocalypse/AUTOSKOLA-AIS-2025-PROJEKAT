using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Data;
using Autoskola.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Autoskola.BLL.Services
{
    public class CasService : ICasService
    {
        private readonly AutoskolaDbContext _context;

        public CasService(AutoskolaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cas>> GetAllAsync()
        {
            return await _context.Casovi
                .Include(c => c.Instruktor)
                .Include(c => c.Vozilo)
                .ToListAsync();
        }

        public async Task<Cas?> GetByIdAsync(int id)
        {
            return await _context.Casovi
                .Include(c => c.Instruktor)
                .Include(c => c.Vozilo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cas cas)
        {
            _context.Casovi.Add(cas);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cas cas)
        {
            _context.Casovi.Update(cas);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cas = await _context.Casovi.FindAsync(id);
            if (cas != null)
            {
                _context.Casovi.Remove(cas);
                await _context.SaveChangesAsync();
            }
        }
    }
}
