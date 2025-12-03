// Controllers/AccesoController.cs
// Login simple para manejar sesión (según ejemplo)
using Microsoft.AspNetCore.Mvc;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso/Index
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel model)
        {
            // Implementación simple: validar usuario hardcode (el PDF tiene un ejemplo similar).
            if (ModelState.IsValid)
            {
                if (model.Usuario == "admin" && model.Password == "admin")
                {
                    HttpContext.Session.SetString("Usuario", model.Usuario);
                    return RedirectToAction("Index", "Turnos");
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrecta.");
            }
            return View(model);
        }

        public IActionResult Salir()
        {
            HttpContext.Session.Remove("Usuario");
            return RedirectToAction("Index");
        }
    }

    public class LoginViewModel
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
