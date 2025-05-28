using System;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class Solicitud
    {
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)] // Oculta este campo en formularios
        public string? UsuarioRegistro { get; set; }  // 👈 HAZLO opcional (string?)

        [Required(ErrorMessage = "El código es obligatorio")]
        public string CodigoSolicitud { get; set; }

        [Required(ErrorMessage = "El detalle es obligatorio")]
        public string Detalle { get; set; }

        public bool Modificado { get; set; } = false;

        public bool Eliminado { get; set; } = false;
    }
}
