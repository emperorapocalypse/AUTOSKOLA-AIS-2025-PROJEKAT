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
    public class VoziloService : IVoziloService
    {
        private readonly AutoskolaDbContext _context;

        public VoziloService(AutoskolaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vozilo>> GetAllAsync()
        {
            return await _context.Vozila
                .Include(v => v.Slike)
                .ToListAsync();
        }

        public async Task<Vozilo?> GetByIdAsync(int id)
        {
            return await _context.Vozila
                .Include(v => v.Slike)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Vozilo vozilo)
        {
            _context.Vozila.Add(vozilo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vozilo vozilo)
        {
            _context.Vozila.Update(vozilo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vozilo = await _context.Vozila
                .Include(v => v.Slike)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vozilo != null)
            {
                _context.Vozila.Remove(vozilo);
                await _context.SaveChangesAsync();
            }
        }

        // Metode za slike
        public async Task<VoziloSlika?> GetSlikaByIdAsync(int id)
        {
            return await _context.VoziloSlike.FindAsync(id);
        }

        public async Task AddSlikaAsync(VoziloSlika slika)
        {
            _context.VoziloSlike.Add(slika);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSlikaAsync(int id)
        {
            var slika = await _context.VoziloSlike.FindAsync(id);
            if (slika != null)
            {
                _context.VoziloSlike.Remove(slika);
                await _context.SaveChangesAsync();
            }
        }
    }
}