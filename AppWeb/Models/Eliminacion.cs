using System;

namespace AppWeb.Models
{
    public class Eliminacion
    {
        public int Id { get; set; }
        public DateTime FechaEliminacion { get; set; } = DateTime.Now;
        public string? Motivo { get; set; }                // ✅ permite nulos
        public string? CodigoSolicitud { get; set; }
        public string? UsuarioSolicitante { get; set; }
        public string? UsuarioEliminador { get; set; }
        public string? Detalle { get; set; }
    }

}
