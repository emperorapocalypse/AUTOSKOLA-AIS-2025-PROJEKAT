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
                    .ThenInclude(v => v.Slike)  
                .Include(c => c.KandidatCasovi)
                    .ThenInclude(kc => kc.Kandidat)
                .ToListAsync();
        }

        public async Task<Cas?> GetByIdAsync(int id)
        {
            return await _context.Casovi
                .Include(c => c.Instruktor)
                .Include(c => c.Vozilo)
                    .ThenInclude(v => v.Slike)  
                .Include(c => c.KandidatCasovi)
                    .ThenInclude(kc => kc.Kandidat)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cas cas, List<int> kandidatIds)
        {
            _context.Casovi.Add(cas);
            await _context.SaveChangesAsync();

           
            if (kandidatIds != null && kandidatIds.Any())
            {
                foreach (var kandidatId in kandidatIds)
                {
                    _context.KandidatCasovi.Add(new KandidatCas
                    {
                        CasId = cas.Id,
                        KandidatId = kandidatId,
                        Prisustvovao = false
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Cas cas, List<(int KandidatId, bool Prisustvovao, string? Napomena)> kandidatCasovi)
        {
            _context.Casovi.Update(cas);

            // Ukloni stare veze sa kandidatima
            var stariKandidati = await _context.KandidatCasovi
                .Where(kc => kc.CasId == cas.Id)
                .ToListAsync();
            _context.KandidatCasovi.RemoveRange(stariKandidati);

            // Dodaj nove veze sa prisustvom i napomenama
            if (kandidatCasovi != null && kandidatCasovi.Any())
            {
                foreach (var (kandidatId, prisustvovao, napomena) in kandidatCasovi)
                {
                    _context.KandidatCasovi.Add(new KandidatCas
                    {
                        CasId = cas.Id,
                        KandidatId = kandidatId,
                        Prisustvovao = prisustvovao,
                        Napomena = napomena
                    });
                }
            }

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

        public async Task<IEnumerable<Instruktor>> GetAllInstruktoriAsync()
        {
            return await _context.Instruktori.ToListAsync();
        }

        public async Task<IEnumerable<Kandidat>> GetAllKandidatiAsync()
        {
            return await _context.Kandidati.ToListAsync();
        }

        public async Task<IEnumerable<Vozilo>> GetAllVozilaAsync()
        {
            return await _context.Vozila
                .Where(v => v.VoznoStanje == VoznoStanje.Vozno)
                .ToListAsync();
        }
    }
}