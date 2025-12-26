using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoskola.MVC.Controllers
{
    public class VoziloController : Controller
    {
        private readonly IVoziloService _voziloService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VoziloController(IVoziloService voziloService, IWebHostEnvironment webHostEnvironment)
        {
            _voziloService = voziloService;
            _webHostEnvironment = webHostEnvironment;
        }

        
        public async Task<IActionResult> Index()
        {
            var vozila = await _voziloService.GetAllAsync();
            return View(vozila);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var vozilo = await _voziloService.GetByIdAsync(id);
            if (vozilo == null)
            {
                TempData["ErrorMessage"] = "Vozilo nije pronađeno.";
                return RedirectToAction(nameof(Index));
            }
            return View(vozilo);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vozilo vozilo, List<IFormFile>? slike, List<string>? opisiSlika)
        {
            ModelState.Remove("Slike");
            ModelState.Remove("Casovi");
            ModelState.Remove("IspitVozila");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    await _voziloService.AddAsync(vozilo);

                    
                    if (slike != null && slike.Any())
                    {
                        await SaveVoziloSlike(vozilo.Id, slike, opisiSlika);
                    }

                    TempData["SuccessMessage"] = "Vozilo je uspešno dodato!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom dodavanja vozila: {ex.Message}";
                }
            }

            return View(vozilo);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var vozilo = await _voziloService.GetByIdAsync(id);
            if (vozilo == null)
            {
                TempData["ErrorMessage"] = "Vozilo nije pronađeno.";
                return RedirectToAction(nameof(Index));
            }
            return View(vozilo);
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vozilo vozilo, List<IFormFile>? noveSlike, List<string>? opisiNovihSlika)
        {
            if (id != vozilo.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Slike");
            ModelState.Remove("Casovi");
            ModelState.Remove("IspitVozila");

            if (ModelState.IsValid)
            {
                try
                {
                    await _voziloService.UpdateAsync(vozilo);

                    
                    if (noveSlike != null && noveSlike.Any())
                    {
                        await SaveVoziloSlike(vozilo.Id, noveSlike, opisiNovihSlika);
                    }

                    TempData["SuccessMessage"] = "Vozilo je uspešno ažurirano!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom ažuriranja vozila: {ex.Message}";
                }
            }

            
            var voziloSaSlikama = await _voziloService.GetByIdAsync(id);
            vozilo.Slike = voziloSaSlikama?.Slike;

            return View(vozilo);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var vozilo = await _voziloService.GetByIdAsync(id);
            if (vozilo == null)
            {
                TempData["ErrorMessage"] = "Vozilo nije pronađeno.";
                return RedirectToAction(nameof(Index));
            }
            return View(vozilo);
        }

        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vozilo = await _voziloService.GetByIdAsync(id);
                if (vozilo != null)
                {
                    
                    if (vozilo.Slike != null && vozilo.Slike.Any())
                    {
                        foreach (var slika in vozilo.Slike)
                        {
                            DeleteImageFile(slika.PutanjaDoSlike);
                        }
                    }

                    await _voziloService.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Vozilo je uspešno obrisano!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom brisanja vozila: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                var slika = await _voziloService.GetSlikaByIdAsync(id);
                if (slika == null)
                {
                    return Json(new { success = false, message = "Slika nije pronađena." });
                }

                
                DeleteImageFile(slika.PutanjaDoSlike);

                
                await _voziloService.DeleteSlikaAsync(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrator")]
        private async Task SaveVoziloSlike(int voziloId, List<IFormFile> slike, List<string>? opisi)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "vozila");

            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            for (int i = 0; i < slike.Count; i++)
            {
                var file = slike[i];
                if (file.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var voziloSlika = new VoziloSlika
                    {
                        VoziloId = voziloId,
                        PutanjaDoSlike = "/uploads/vozila/" + uniqueFileName,
                        Opis = opisi != null && i < opisi.Count ? opisi[i] : null
                    };

                    await _voziloService.AddSlikaAsync(voziloSlika);
                }
            }
        }
        [Authorize(Roles = "Administrator")]
        private void DeleteImageFile(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}