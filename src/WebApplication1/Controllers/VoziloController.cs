using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
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

        // GET: Vozilo
        public async Task<IActionResult> Index()
        {
            var vozila = await _voziloService.GetAllAsync();
            return View(vozila);
        }

        // GET: Vozilo/Details/5
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

        // GET: Vozilo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vozilo/Create
        [HttpPost]
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
                    // Sačuvaj vozilo prvo
                    await _voziloService.AddAsync(vozilo);

                    // Dodaj slike ako postoje
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

        // GET: Vozilo/Edit/5
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

        // POST: Vozilo/Edit/5
        [HttpPost]
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

                    // Dodaj nove slike ako postoje
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

            // Ponovo učitaj vozilo sa slikama za prikaz
            var voziloSaSlikama = await _voziloService.GetByIdAsync(id);
            vozilo.Slike = voziloSaSlikama?.Slike;

            return View(vozilo);
        }

        // GET: Vozilo/Delete/5
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

        // POST: Vozilo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vozilo = await _voziloService.GetByIdAsync(id);
                if (vozilo != null)
                {
                    // Obriši sve slike sa diska
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

        // POST: Vozilo/DeleteImage/5
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                var slika = await _voziloService.GetSlikaByIdAsync(id);
                if (slika == null)
                {
                    return Json(new { success = false, message = "Slika nije pronađena." });
                }

                // Obriši fajl sa diska
                DeleteImageFile(slika.PutanjaDoSlike);

                // Obriši iz baze
                await _voziloService.DeleteSlikaAsync(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper metode
        private async Task SaveVoziloSlike(int voziloId, List<IFormFile> slike, List<string>? opisi)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "vozila");

            // Kreiraj folder ako ne postoji
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