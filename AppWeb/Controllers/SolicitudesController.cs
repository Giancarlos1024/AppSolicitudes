using Microsoft.AspNetCore.Mvc;
using AppWeb.Data;
using Microsoft.AspNetCore.Http;
using AppWeb.Models;
using System.Linq;

namespace AppWeb.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var solicitudes = _context.Solicitudes
                .Where(s => !s.Eliminado)
                .OrderByDescending(s => s.FechaRegistro)
                .ToList();

            return View(solicitudes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Solicitud solicitud)
        {
            Console.WriteLine("🔍 Entrando a POST Create");

            // ✅ Validar que el usuario esté autenticado (seguridad + evitar null)
            var usuario = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuario))
            {
                Console.WriteLine("⚠️ Usuario no autenticado. Redirigiendo a Login.");
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                // ✅ Verificar código duplicado
                bool codigoDuplicado = _context.Solicitudes.Any(s => s.CodigoSolicitud == solicitud.CodigoSolicitud);
                if (codigoDuplicado)
                {
                    ModelState.AddModelError("CodigoSolicitud", "Ya existe una solicitud con este código.");
                    return View(solicitud);
                }

                solicitud.UsuarioRegistro = usuario;

                Console.WriteLine($"📌 Usuario: {solicitud.UsuarioRegistro}");
                Console.WriteLine($"📌 Código: {solicitud.CodigoSolicitud}");

                try
                {
                    _context.Solicitudes.Add(solicitud);
                    _context.SaveChanges();

                    TempData["Mensaje"] = "✅ Solicitud registrada correctamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al guardar: {ex.Message}");
                    ModelState.AddModelError("", "Ocurrió un error al guardar la solicitud.");
                }
            }
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"❌ Error en '{key}': {error.ErrorMessage}");
                }
            }

            return View(solicitud);
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var solicitud = _context.Solicitudes.FirstOrDefault(s => s.Id == id && !s.Eliminado);
            if (solicitud == null) return NotFound();
            return View(solicitud);
        }

        [HttpPost]
        public IActionResult Edit(Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                var original = _context.Solicitudes.FirstOrDefault(s => s.Id == solicitud.Id);
                if (original != null)
                {
                    original.CodigoSolicitud = solicitud.CodigoSolicitud;
                    original.Detalle = solicitud.Detalle;
                    original.Modificado = true;
                    _context.SaveChanges();
                    TempData["Mensaje"] = "Solicitud modificada correctamente.";
                    return RedirectToAction("Index");
                }
            }
            return View(solicitud);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var solicitud = _context.Solicitudes.FirstOrDefault(s => s.Id == id && !s.Eliminado);
            if (solicitud == null) return NotFound();


            return View(solicitud);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id, string motivo)
        {
            var solicitud = _context.Solicitudes.FirstOrDefault(s => s.Id == id);
            if (solicitud != null)
            {
                var usuarioActual = HttpContext.Session.GetString("Usuario") ?? "Desconocido";

                var eliminacion = new Eliminacion
                {
                    FechaEliminacion = DateTime.Now,
                    Motivo = string.IsNullOrWhiteSpace(motivo) ? "Sin motivo especificado" : motivo.Trim(),
                    CodigoSolicitud = solicitud.CodigoSolicitud,
                    UsuarioSolicitante = solicitud.UsuarioRegistro,
                    UsuarioEliminador = usuarioActual,
                    Detalle = solicitud.Detalle
                };

                solicitud.Eliminado = true;
                _context.Eliminaciones.Add(eliminacion);
                _context.SaveChanges();

                TempData["Mensaje"] = "Solicitud eliminada.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminadas()
        {
            var eliminadas = _context.Eliminaciones
                .OrderByDescending(e => e.FechaEliminacion)
                .ToList();

            return View(eliminadas);
        }

    }
}
