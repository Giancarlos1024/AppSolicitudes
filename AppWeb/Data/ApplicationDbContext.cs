using Microsoft.EntityFrameworkCore;
using AppWeb.Models;
using System.Collections.Generic;

namespace AppWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Eliminacion> Eliminaciones { get; set; }
    }
}
