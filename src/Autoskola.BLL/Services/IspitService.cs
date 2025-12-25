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
    public class IspitService : IIspitService
    {
        private readonly AutoskolaDbContext _context;

        public IspitService(AutoskolaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ispit>> GetAllAsync()
        {
            return await _context.Ispiti
                .Include(i => i.Instruktor)
                .ToListAsync();
        }

        public async Task<Ispit?> GetByIdAsync(int id)
        {
            return await _context.Ispiti
                .Include(i => i.Instruktor)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Ispit ispit)
        {
            _context.Ispiti.Add(ispit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ispit ispit)
        {
            _context.Ispiti.Update(ispit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ispit = await _context.Ispiti.FindAsync(id);
            if (ispit != null)
            {
                _context.Ispiti.Remove(ispit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
