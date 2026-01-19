using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
using Autoskola.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoskola.MVC.Controllers
{
    public class KandidatController : Controller
    {
        private readonly IKandidatService _kandidatService;
        private readonly IFileUploadService _fileUploadService;

        public KandidatController(
            IKandidatService kandidatService,
            IFileUploadService fileUploadService)
        {
            _kandidatService = kandidatService;
            _fileUploadService = fileUploadService;
        }

        
        public async Task<IActionResult> Index()
        {
            var kandidati = await _kandidatService.GetAllAsync();
            return View(kandidati);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var kandidat = await _kandidatService.GetByIdAsync(id);
            if (kandidat == null)
                return NotFound();

            return View(kandidat);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kandidat kandidat, IFormFile profilnaSlika)
        {
            try
            {
                ModelState.Clear();

                
                if (profilnaSlika != null && profilnaSlika.Length > 0)
                {
                    kandidat.ProfilnaSlika = await _fileUploadService.UploadImageAsync(profilnaSlika, "kandidati");
                }

                await _kandidatService.AddAsync(kandidat);

                TempData["SuccessMessage"] = $"Kandidat {kandidat.Ime} {kandidat.Prezime} je uspešno dodat!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
                return View(kandidat);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var kandidat = await _kandidatService.GetByIdAsync(id);
            if (kandidat == null)
            {
                TempData["ErrorMessage"] = "Kandidat nije pronađen.";
                return RedirectToAction(nameof(Index));
            }

            return View(kandidat);
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kandidat kandidat, IFormFile profilnaSlika)
        {
            if (id != kandidat.Id)
            {
                TempData["ErrorMessage"] = "Neispravni podaci.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
            
                if (profilnaSlika != null && profilnaSlika.Length > 0)
                {
                    
                    if (!string.IsNullOrEmpty(kandidat.ProfilnaSlika))
                    {
                        await _fileUploadService.DeleteImageAsync(kandidat.ProfilnaSlika);
                    }

                   
                    kandidat.ProfilnaSlika = await _fileUploadService.UploadImageAsync(profilnaSlika, "kandidati");
                }
               

                await _kandidatService.UpdateAsync(kandidat);

                TempData["SuccessMessage"] = $"Kandidat {kandidat.Ime} {kandidat.Prezime} je uspešno izmenjen!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom izmene: {ex.Message}";
                return View(kandidat);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var kandidat = await _kandidatService.GetByIdAsync(id);
            if (kandidat == null)
                return NotFound();

            return View(kandidat);
        }

        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var kandidat = await _kandidatService.GetByIdAsync(id);

                
                if (kandidat != null && !string.IsNullOrEmpty(kandidat.ProfilnaSlika))
                {
                    await _fileUploadService.DeleteImageAsync(kandidat.ProfilnaSlika);
                }

                await _kandidatService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Kandidat je uspešno obrisan!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom brisanja: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}