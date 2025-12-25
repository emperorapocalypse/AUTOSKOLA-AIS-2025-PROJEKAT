using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Autoskola.MVC.Controllers
{
    public class CasController : Controller
    {
        private readonly ICasService _casService;

        public CasController(ICasService casService)
        {
            _casService = casService;
        }

      
        public async Task<IActionResult> Index()
        {
            var casovi = await _casService.GetAllAsync();
            return View(casovi);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var cas = await _casService.GetByIdAsync(id);
            if (cas == null)
            {
                TempData["ErrorMessage"] = "Čas nije pronađen.";
                return RedirectToAction(nameof(Index));
            }
            return View(cas);
        }


        public async Task<IActionResult> Create()
        {
            var instruktori = await _casService.GetAllInstruktoriAsync();
            var vozila = await _casService.GetAllVozilaAsync();

            ViewBag.InstruktoriLista = instruktori.ToList();
            ViewBag.VozilaLista = vozila.ToList();
            ViewBag.Kandidati = await _casService.GetAllKandidatiAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cas cas, List<int>? selectedKandidati)
        {
            
            ModelState.Remove("Instruktor");
            ModelState.Remove("Vozilo");
            ModelState.Remove("KandidatCasovi");

            if (ModelState.IsValid)
            {
                try
                {
                    await _casService.AddAsync(cas, selectedKandidati ?? new List<int>());
                    TempData["SuccessMessage"] = "Čas je uspešno dodat!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom dodavanja časa: {ex.Message}";
                }
            }

            var instruktori = await _casService.GetAllInstruktoriAsync();
            var vozila = await _casService.GetAllVozilaAsync();

            ViewBag.InstruktoriLista = instruktori.ToList();
            ViewBag.VozilaLista = vozila.ToList();
            ViewBag.Kandidati = await _casService.GetAllKandidatiAsync();

            return View(cas);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var cas = await _casService.GetByIdAsync(id);
            if (cas == null)
            {
                TempData["ErrorMessage"] = "Čas nije pronađen.";
                return RedirectToAction(nameof(Index));
            }

            var instruktori = await _casService.GetAllInstruktoriAsync();
            var vozila = await _casService.GetAllVozilaAsync();

            ViewBag.InstruktoriLista = instruktori.ToList();
            ViewBag.VozilaLista = vozila.ToList();
            ViewBag.Kandidati = await _casService.GetAllKandidatiAsync();
            ViewBag.SelectedKandidati = cas.KandidatCasovi?.Select(kc => kc.KandidatId).ToList() ?? new List<int>();

            return View(cas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cas cas, List<int>? selectedKandidati)
        {
            if (id != cas.Id)
            {
                return NotFound();
            }

            // KRITIČNO: Ukloni validaciju za navigation properties
            ModelState.Remove("Instruktor");
            ModelState.Remove("Vozilo");
            ModelState.Remove("KandidatCasovi");

            if (ModelState.IsValid)
            {
                try
                {
                    // Očitaj prisustva i napomene iz forme
                    var kandidatCasoviData = new List<(int KandidatId, bool Prisustvovao, string? Napomena)>();

                    if (selectedKandidati != null)
                    {
                        foreach (var kandidatId in selectedKandidati)
                        {
                            bool prisustvovao = Request.Form[$"prisustva_{kandidatId}"].ToString() == "on";
                            string? napomena = Request.Form[$"napomene_{kandidatId}"].ToString();

                            kandidatCasoviData.Add((kandidatId, prisustvovao, napomena));
                        }
                    }

                    await _casService.UpdateAsync(cas, kandidatCasoviData);
                    TempData["SuccessMessage"] = "Čas je uspešno ažuriran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom ažuriranja časa: {ex.Message}";
                }
            }
            else
            {
                // Debug - prikaži greške
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = "Greške validacije: " + string.Join(", ", errors);
            }

            var instruktori = await _casService.GetAllInstruktoriAsync();
            var vozila = await _casService.GetAllVozilaAsync();

            ViewBag.InstruktoriLista = instruktori.ToList();
            ViewBag.VozilaLista = vozila.ToList();
            ViewBag.Kandidati = await _casService.GetAllKandidatiAsync();
            ViewBag.SelectedKandidati = selectedKandidati ?? new List<int>();

            return View(cas);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var cas = await _casService.GetByIdAsync(id);
            if (cas == null)
            {
                TempData["ErrorMessage"] = "Čas nije pronađen.";
                return RedirectToAction(nameof(Index));
            }
            return View(cas);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _casService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Čas je uspešno obrisan!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom brisanja časa: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }



    }
}