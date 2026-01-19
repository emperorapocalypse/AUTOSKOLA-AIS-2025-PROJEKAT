using Autoskola.BLL.Interfaces;
using Autoskola.DAL.Models;
using Autoskola.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autoskola.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IKandidatService _kandidatService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IKandidatService kandidatService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _kandidatService = kandidatService;
        }

        
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Neispravno korisničko ime ili lozinka.");
                }
            }

            return View(model);
        }

        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Register()
        {
            
            ViewBag.Kandidati = await _kandidatService.GetAllAsync();
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.KandidatId.HasValue && model.KandidatId.Value > 0)
                {
                   
                    var existingUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.KandidatId == model.KandidatId.Value);

                    if (existingUser != null)
                    {
                        ModelState.AddModelError("", "Ovaj kandidat već ima kreiran nalog!");
                        ViewBag.Kandidati = await _kandidatService.GetAllAsync();
                        return View(model);
                    }
                }

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    KandidatId = model.KandidatId > 0 ? model.KandidatId : null
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    
                    if (model.KandidatId.HasValue && model.KandidatId.Value > 0)
                    {
                        await _userManager.AddToRoleAsync(user, "Kandidat");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Administrator");
                    }

                    TempData["SuccessMessage"] = $"Korisnik {user.UserName} uspešno kreiran!";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            
            ViewBag.Kandidati = await _kandidatService.GetAllAsync();
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}