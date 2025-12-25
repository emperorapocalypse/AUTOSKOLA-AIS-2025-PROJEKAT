using Autoskola.BLL.Interfaces;
using Autoskola.DAL;
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
                .Include(i => i.KandidatIspiti)
                    .ThenInclude(ki => ki.Kandidat)
                .Include(i => i.IspitVozila)
                    .ThenInclude(iv => iv.Vozilo)
                        .ThenInclude(v => v.Slike)
                .ToListAsync();
        }

        public async Task<Ispit?> GetByIdAsync(int id)
        {
            return await _context.Ispiti
                .Include(i => i.Instruktor)
                .Include(i => i.KandidatIspiti)
                    .ThenInclude(ki => ki.Kandidat)
                .Include(i => i.IspitVozila)
                    .ThenInclude(iv => iv.Vozilo)
                        .ThenInclude(v => v.Slike)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Ispit ispit, int kandidatId, List<int>? vozilaIds)
        {
           
            if (ispit.TipIspita == TipIspita.Praktican && (vozilaIds == null || !vozilaIds.Any()))
            {
                throw new InvalidOperationException("Praktičan ispit mora imati bar jedno vozilo!");
            }

            
            if (ispit.TipIspita == TipIspita.Teorijski && vozilaIds?.Any() == true)
            {
                throw new InvalidOperationException("Teorijski ispit ne može imati vozila!");
            }

            _context.Ispiti.Add(ispit);
            await _context.SaveChangesAsync();

            _context.KandidatIspiti.Add(new KandidatIspit
            {
                IspitId = ispit.Id,
                KandidatId = kandidatId
            });

            if (vozilaIds?.Any() == true)
            {
                foreach (var vid in vozilaIds)
                {
                    _context.IspitVozila.Add(new IspitVozilo
                    {
                        IspitId = ispit.Id,
                        VoziloId = vid
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ispit ispit, int kandidatId, List<int>? vozilaIds)
        {
            
            if (ispit.TipIspita == TipIspita.Praktican && (vozilaIds == null || !vozilaIds.Any()))
            {
                throw new InvalidOperationException("Praktičan ispit mora imati bar jedno vozilo!");
            }

           
            if (ispit.TipIspita == TipIspita.Teorijski && vozilaIds?.Any() == true)
            {
                throw new InvalidOperationException("Teorijski ispit ne može imati vozila!");
            }

            _context.Ispiti.Update(ispit);

            _context.KandidatIspiti.RemoveRange(
                await _context.KandidatIspiti.Where(ki => ki.IspitId == ispit.Id).ToListAsync()
            );
            _context.IspitVozila.RemoveRange(
                await _context.IspitVozila.Where(iv => iv.IspitId == ispit.Id).ToListAsync()
            );

            _context.KandidatIspiti.Add(new KandidatIspit
            {
                IspitId = ispit.Id,
                KandidatId = kandidatId
            });

            if (vozilaIds?.Any() == true)
            {
                foreach (var vid in vozilaIds)
                {
                    _context.IspitVozila.Add(new IspitVozilo
                    {
                        IspitId = ispit.Id,
                        VoziloId = vid
                    });
                }
            }

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
                .Include(v => v.Slike)
                .ToListAsync();
        }
    }
}