using Autoskola.BLL.Interfaces;
using Autoskola.MVC.Services;
using Autoskola.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autoskola.MVC.Controllers
{
    public class InstruktorController : Controller
    {
        private readonly IInstruktorService _instruktorService;
        private readonly IFileUploadService _fileUploadService;

        public InstruktorController(
            IInstruktorService instruktorService,
            IFileUploadService fileUploadService)
        {
            _instruktorService = instruktorService;
            _fileUploadService = fileUploadService;
        }

     
        public async Task<IActionResult> Index()
        {
            var instruktori = await _instruktorService.GetAllAsync();
            return View(instruktori);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var instruktor = await _instruktorService.GetByIdAsync(id);
            if (instruktor == null)
                return NotFound();

            return View(instruktor);
        }

       
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instruktor instruktor, IFormFile profilnaSlika)
        {
            try
            {
                ModelState.Clear();

                
                if (profilnaSlika != null && profilnaSlika.Length > 0)
                {
                    instruktor.ProfilnaSlika = await _fileUploadService.UploadImageAsync(profilnaSlika, "instruktori");
                }

                await _instruktorService.AddAsync(instruktor);

                TempData["SuccessMessage"] = $"Instruktor {instruktor.Ime} {instruktor.Prezime} je uspešno dodat!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
                return View(instruktor);
            }
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var instruktor = await _instruktorService.GetByIdAsync(id);
            if (instruktor == null)
            {
                TempData["ErrorMessage"] = "Instruktor nije pronađen.";
                return RedirectToAction(nameof(Index));
            }

            return View(instruktor);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instruktor instruktor, IFormFile profilnaSlika)
        {
            if (id != instruktor.Id)
            {
                TempData["ErrorMessage"] = "Neispravni podaci.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
              
                if (profilnaSlika != null && profilnaSlika.Length > 0)
                {
                    // Obriši staru sliku ako postoji
                    if (!string.IsNullOrEmpty(instruktor.ProfilnaSlika))
                    {
                        await _fileUploadService.DeleteImageAsync(instruktor.ProfilnaSlika);
                    }

                   
                    instruktor.ProfilnaSlika = await _fileUploadService.UploadImageAsync(profilnaSlika, "instruktori");
                }

                await _instruktorService.UpdateAsync(instruktor);

                TempData["SuccessMessage"] = $"Instruktor {instruktor.Ime} {instruktor.Prezime} je uspešno izmenjen!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom izmene: {ex.Message}";
                return View(instruktor);
            }
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            var instruktor = await _instruktorService.GetByIdAsync(id);
            if (instruktor == null)
                return NotFound();

            return View(instruktor);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var instruktor = await _instruktorService.GetByIdAsync(id);

                // Obriši sliku ako postoji
                if (instruktor != null && !string.IsNullOrEmpty(instruktor.ProfilnaSlika))
                {
                    await _fileUploadService.DeleteImageAsync(instruktor.ProfilnaSlika);
                }

                await _instruktorService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Instruktor je uspešno obrisan!";
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