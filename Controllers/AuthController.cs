using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.DTOs;
using ProjetSynthese.Server.Models.Login;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthController> _logger;
        private readonly MyDbContext _context;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IEmailSender emailSender, ILogger<AuthController> logger, MyDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Number);
            if (user == null)
                return Unauthorized("Échec de la connexion");

            var result = await _signInManager.PasswordSignInAsync(model.Number, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                return Ok("Connexion réussie");

            return Unauthorized("Échec de la connexion");
        }

        [Authorize]
        [HttpGet("getInfo")]
        public async Task<IActionResult> GetInfo()
        {
            // Vérifie que l'utilisateur existe
            IdentityUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                Enseignant enseignant = await _context.Enseignants.FirstOrDefaultAsync(e => e.UtilisateurId == user.Id);
                return Ok(
                    new UserInfoDto() {
                        Id = user.Id,
                        Courriel = user.Email,
                        Nom = enseignant.Nom,
                        Prenom = enseignant.Prenom,
                        Role = "Admin",
                    }
                );
            }
            else if (await _userManager.IsInRoleAsync(user, "Enseignant"))
            {
                Enseignant enseignant = await _context.Enseignants.FirstOrDefaultAsync(e => e.UtilisateurId == user.Id);
                return Ok(
                    new UserInfoDto()
                    {
                        Id = user.Id,
                        Courriel = user.Email,
                        Nom = enseignant.Nom,
                        Prenom = enseignant.Prenom,
                        Role = "Enseignant",
                    }
                );
            }
            else if (await _userManager.IsInRoleAsync(user, "Etudiant"))
            {
                Etudiant etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == user.Id);
                return Ok(
                    new UserInfoDto()
                    {
                        Id = user.Id,
                        Courriel = user.Email,
                        Nom = etudiant.Nom,
                        Prenom = etudiant.Prenom,
                        Role = "Etudiant",
                    }
                );
            }
            else
            {
                return BadRequest("Rôle utilisateur inconnu.");
            }
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Registre model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            model.Nom = model.Nom?.Trim();
            model.Prenom = model.Prenom?.Trim();

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Courriel);
            if (existingUser != null)
            {
                return BadRequest("Un utilisateur avec ce courriel existe déjà.");
            }

            var user = new IdentityUser { UserName = model.Numero, Email = model.Courriel };
            var result = await _userManager.CreateAsync(user, model.MotDePasse);

            if (model.Numero.Length == 7)
            {
                await _userManager.AddToRoleAsync(user, "Etudiant");
                Etudiant etudiant = new Etudiant();
                etudiant.UtilisateurId = user.Id;
                etudiant.utilisateur = user;
                etudiant.numeroEtudiant = model.Numero;
                etudiant.Nom = model.Nom;
                etudiant.Prenom = model.Prenom;

                try
                {
                    _context.Etudiants.Add(etudiant);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'étudiant.");
                }
            }
            else if (model.Numero.Length == 5)
            {
                await _userManager.AddToRoleAsync(user, "Enseignant");
                Enseignant enseignant = new Enseignant();
                enseignant.UtilisateurId = user.Id;
                enseignant.utilisateur = user;
                enseignant.NoEnseignant = model.Numero;
                enseignant.Nom = model.Nom;
                enseignant.Prenom = model.Prenom;

                try
                {
                    _context.Enseignants.Add(enseignant);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'enseignant.");
                }
            }

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var param = new Dictionary<string, string?>
                {
                    {"token", token },
                    {"courriel", model.Courriel }
                };

                var callback = QueryHelpers.AddQueryString("https://localhost:4200/confirmation-courriel", param);

                var sendEmailTask = _emailSender.SendEmailAsync(model.Courriel, "Confirmation de votre courriel", "Veuillez confirmer votre courriel en cliquant <a href='" + callback + "'>sur ce lien</a>.");

                // @TODO condition pour byopass l'envoi d'email en attendant qu'on puisse envoyer un email qui fonctionne.
                if (await Task.WhenAny(sendEmailTask, Task.Delay(3000)) == sendEmailTask)
                {
                    // Email sending completed within the timeout
                    return Ok();
                }
                else
                {
                    return Ok();
                }


                //Voir appsettings.json pour changer les "credentials" du courriel.
                //await _emailSender.SendEmailAsync(model.Courriel, "Confirmation de votre courriel", "Veuillez confirmer votre courriel en cliquant <a href='" + callback + "'>sur ce lien</a>.");
                return Ok();
            }



            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("ConfirmationCourriel")]
        public async Task<IActionResult> ConfirmationCourriel([FromQuery] string token, [FromQuery] string courriel)
        {
            var decodedToken = Uri.UnescapeDataString(token);
            var decodedcourriel = Uri.UnescapeDataString(courriel);

            var user = await _userManager.FindByEmailAsync(decodedcourriel);
            if (user == null)
                return BadRequest("Requête de confirmaiton de courriel invalide");
            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!confirmResult.Succeeded)
                return BadRequest("Requête de confirmaiton de courriel invalide");
            return Ok();
        }
    }
}
