using Microsoft.EntityFrameworkCore;
using AppWeb.Data;

namespace AppWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de la base de datos
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar MVC
            builder.Services.AddControllersWithViews();

            // Configurar sesión correctamente (SOLO UNA VEZ)
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".AppWeb.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            // Servicio de acceso a HttpContext
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Verificar conexión a la base de datos
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.CanConnect();
                    Console.WriteLine("✅ Conexión a la base de datos establecida correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al conectar a la base de datos:");
                    Console.WriteLine(ex.Message);
                }
            }

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //Importante: sesión ANTES de autorización
            app.UseSession();

            app.UseAuthorization();

            // Ruta por defecto
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
