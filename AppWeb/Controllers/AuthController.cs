using Microsoft.AspNetCore.Mvc;
using AppWeb.Models;
using AppWeb.Data;
using System.Linq;

namespace AppWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuarioNombre, string contrasena)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre && u.Contrasena == contrasena);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
                return RedirectToAction("Index", "Solicitudes");
            }

            ViewBag.Mensaje = "Credenciales incorrectas";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string usuarioNombre, string contrasena)
        {
            if (_context.Usuarios.Any(u => u.NombreUsuario == usuarioNombre))
            {
                ViewBag.Mensaje = "El usuario ya existe.";
                return View();
            }

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = usuarioNombre,
                Contrasena = contrasena
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            // Opcional: loguear automáticamente al registrar
            HttpContext.Session.SetString("Usuario", usuarioNombre);
            return RedirectToAction("Index", "Solicitudes");
        }

    }
}
