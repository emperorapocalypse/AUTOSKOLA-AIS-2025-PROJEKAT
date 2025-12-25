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
    public class InstruktorService : IInstruktorService
    {
        private readonly AutoskolaDbContext _context;

        public InstruktorService(AutoskolaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Instruktor>> GetAllAsync()
        {
            return await _context.Instruktori.ToListAsync();
        }

        public async Task<Instruktor?> GetByIdAsync(int id)
        {
            return await _context.Instruktori.FindAsync(id);
        }

        public async Task AddAsync(Instruktor instruktor)
        {
            _context.Instruktori.Add(instruktor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instruktor instruktor)
        {
            _context.Instruktori.Update(instruktor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var instruktor = await _context.Instruktori.FindAsync(id);
            if (instruktor != null)
            {
                _context.Instruktori.Remove(instruktor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
