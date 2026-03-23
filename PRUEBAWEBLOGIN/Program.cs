using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRUEBAWEBLOGIN.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configuración de la base de datos SQL Server (LocalDB)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LoginAppTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));

// Configuración de autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromSeconds(60); // 
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Creación de la base de datos e inserción de datos desde el script SQL
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    // Insertar datos de prueba si la tabla está vacía
    if (!db.Users.Any())
    {
        var sqlPath = Path.Combine(app.Environment.ContentRootPath, "SQL", "LoginAppTestDb_Schema.sql");
        if (File.Exists(sqlPath))
        {
            var sql = File.ReadAllText(sqlPath);
            // Extraer solo las sentencias INSERT (el esquema ya fue creado por EF)
            var lines = sql.Split('\n');
            var insertSql = new System.Text.StringBuilder();
            bool inInsert = false;
            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("INSERT INTO", StringComparison.OrdinalIgnoreCase))
                    inInsert = true;
                if (inInsert)
                {
                    insertSql.AppendLine(line);
                    if (line.Contains(");"))
                    {
                        inInsert = false;
                        db.Database.ExecuteSqlRaw(insertSql.ToString());
                        insertSql.Clear();
                    }
                }
            }
        }
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Ruta por defecto: inicia en la página de bienvenida
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();
