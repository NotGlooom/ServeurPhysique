using Microsoft.AspNetCore.Identity;
using ProjetSynthese.Server.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjetSynthese.Server.Services;
using System.Diagnostics;
using ProjetSynthese.Server.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for easier reading of JSON in responses
    });

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly); // Ajouter le mapper pour les DTO

builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Confirmation du compte requise
    options.User.RequireUniqueEmail = true; // Nécessite des adresses e-mail uniques
})
.AddRoles<IdentityRole>() // Ajoute la gestion des rôles
.AddEntityFrameworkStores<MyDbContext>();

// Enregistre les services pour le DI
builder.Services.AddScoped<ILatexService, LatexService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IImageExtractionService, ImageExtractionService>();

// Ajouter CourrielSender en tant que service pour l'envoi d'emails
builder.Services.AddTransient<IEmailSender, CourrielSender>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("https://jolly-dune-0b4e4090f.5.azurestaticapps.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // non accessible via javascript
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS seulement
    options.Cookie.SameSite = SameSiteMode.Lax; // cross site accessible
    options.Cookie.Name = "LoginCookie_Exercices_de_Physique";
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

// Installation des packages python
InstallPythonPackages(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    InitialiseurBD.Seed(app);
}

app.Run();

void InstallPythonPackages(IConfiguration configuration)
{
    string pythonPath = PythonHelper.GetPythonPath(configuration);
    InstallPythonPackage(pythonPath, "sympy");
    InstallPythonPackage(pythonPath, "antlr4-python3-runtime==4.11");
}

void InstallPythonPackage(string pythonPath, string packageName)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = pythonPath,
        Arguments = $"-m pip install {packageName}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using (var process = new Process { StartInfo = startInfo })
    {
        try
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Log les erreurs
            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Erreur durant l'installation du package '{packageName}': {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur est survenu durant l'installation du package python '{packageName}': {ex.Message}");
        }
    }
}