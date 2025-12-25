using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autoskola.Controllers
{
    public class IspitController : Controller
    {
        private readonly IIspitService _ispitService;

        public IspitController(IIspitService ispitService)
        {
            _ispitService = ispitService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _ispitService.GetAllAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var ispit = await _ispitService.GetByIdAsync(id);
            if (ispit == null)
            {
                TempData["ErrorMessage"] = "Ispit nije pronađen.";
                return RedirectToAction(nameof(Index));
            }
            return View(ispit);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.InstruktoriLista = (await _ispitService.GetAllInstruktoriAsync()).ToList();
            ViewBag.VozilaLista = (await _ispitService.GetAllVozilaAsync()).ToList();
            ViewBag.Kandidati = await _ispitService.GetAllKandidatiAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ispit ispit, int kandidatId, List<int>? selectedVozila)
        {
            ModelState.Remove("Instruktor");
            ModelState.Remove("KandidatIspiti");
            ModelState.Remove("IspitVozila");

            if (ModelState.IsValid)
            {
                try
                {
                    await _ispitService.AddAsync(ispit, kandidatId, selectedVozila);
                    TempData["SuccessMessage"] = "Ispit uspešno dodat!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška: {ex.Message}";
                }
            }

            ViewBag.InstruktoriLista = (await _ispitService.GetAllInstruktoriAsync()).ToList();
            ViewBag.VozilaLista = (await _ispitService.GetAllVozilaAsync()).ToList();
            ViewBag.Kandidati = await _ispitService.GetAllKandidatiAsync();
            return View(ispit);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ispit = await _ispitService.GetByIdAsync(id);
            if (ispit == null)
            {
                TempData["ErrorMessage"] = "Ispit nije pronađen.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.InstruktoriLista = (await _ispitService.GetAllInstruktoriAsync()).ToList();
            ViewBag.VozilaLista = (await _ispitService.GetAllVozilaAsync()).ToList();
            ViewBag.Kandidati = await _ispitService.GetAllKandidatiAsync();
            ViewBag.SelectedKandidatId = ispit.KandidatIspiti.FirstOrDefault()?.KandidatId ?? 0;
            ViewBag.SelectedVozila = ispit.IspitVozila.Select(iv => iv.VoziloId).ToList();
            return View(ispit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ispit ispit, int kandidatId, List<int>? selectedVozila)
        {
            if (id != ispit.Id) return NotFound();

            ModelState.Remove("Instruktor");
            ModelState.Remove("KandidatIspiti");
            ModelState.Remove("IspitVozila");

            if (ModelState.IsValid)
            {
                try
                {
                    await _ispitService.UpdateAsync(ispit, kandidatId, selectedVozila);
                    TempData["SuccessMessage"] = "Ispit ažuriran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška: {ex.Message}";
                }
            }

            ViewBag.InstruktoriLista = (await _ispitService.GetAllInstruktoriAsync()).ToList();
            ViewBag.VozilaLista = (await _ispitService.GetAllVozilaAsync()).ToList();
            ViewBag.Kandidati = await _ispitService.GetAllKandidatiAsync();
            ViewBag.SelectedKandidatId = kandidatId;
            ViewBag.SelectedVozila = selectedVozila;
            return View(ispit);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ispit = await _ispitService.GetByIdAsync(id);
            if (ispit == null)
            {
                TempData["ErrorMessage"] = "Ispit nije pronađen.";
                return RedirectToAction(nameof(Index));
            }
            return View(ispit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _ispitService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Ispit obrisan!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}