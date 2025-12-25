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
    public class KandidatService : IKandidatService
    {
        private readonly AutoskolaDbContext _context;

        public KandidatService(AutoskolaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kandidat>> GetAllAsync()
        {
            return await _context.Kandidati.ToListAsync();
        }

        public async Task<Kandidat?> GetByIdAsync(int id)
        {
            return await _context.Kandidati.FindAsync(id);
        }

        public async Task AddAsync(Kandidat kandidat)
        {
            _context.Kandidati.Add(kandidat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Kandidat kandidat)
        {
            Console.WriteLine($"=== SERVICE: Ažuriram kandidata ID={kandidat.Id} ===");

            _context.Kandidati.Update(kandidat);
            await _context.SaveChangesAsync();

            Console.WriteLine("=== SERVICE: Kandidat ažuriran! ===");
        }

        public async Task DeleteAsync(int id)
        {
            var kandidat = await _context.Kandidati.FindAsync(id);
            if (kandidat != null)
            {
                _context.Kandidati.Remove(kandidat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
