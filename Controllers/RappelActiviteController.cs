using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Etudiant")]
    public class RappelActiviteController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RappelActiviteController(UserManager<IdentityUser> userManager, MyDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        // Retourne la liste des rappels de l'utilisateur
        public async Task<IActionResult> GetListRappelActiviteAsync()
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Accède au rappels
                var rappels = await _context.RappelsActivite
                    .Where(r => r.UtilisateurId == etudiantId)
                    .ToListAsync();

                return Ok(rappels);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des rappels.");
            }
        }

        [HttpGet("{id}", Name = "GetRappelActivite")]
        public async Task<ActionResult<RappelActivite>> GetRappelActiviteAsync(int id)
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Accède au rappel
                var rappelActivite = await _context.RappelsActivite
                    .FirstOrDefaultAsync(r => r.Id == id && r.UtilisateurId == etudiantId);

                if (rappelActivite == null)
                {
                    return NotFound("Aucun rappel d'activité qui vous appartient trouvée avec cet id.");
                }

                return Ok(rappelActivite);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération du rappel.");
            }
        }

        [HttpGet("activite")]
        // Retourne la liste des rappels de l'utilisateur lié à une activité
        public async Task<IActionResult> GetListRappelFromActiviteAsync([FromQuery] int id)
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Accède au rappels
                var rappels = await _context.RappelsActivite
                    .Where(r => r.UtilisateurId == etudiantId)
                    .Where(r => r.ActiviteId == id)
                    .ToListAsync();

                return Ok(rappels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des rappels liés à l'activité.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRappelActiviteAsync(int id, RappelActivite rappelActivite)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Vérifie que l'id n'a pas changer
                if (id != rappelActivite.Id)
                {
                    return BadRequest("L'ID du rappel ne correspond pas.");
                }

                // Vérifie que c'est son rappel
                var existingRappelActivite = await _context.RappelsActivite
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (existingRappelActivite == null)
                {
                    return NotFound("Rappel d'activité pas trouvé.");
                }

                if (existingRappelActivite.UtilisateurId != etudiantId)
                {
                    return Forbid("Vous n'êtes pas autorisé à modifier ce rappel d'activité.");
                }

                // Modifie le rappel
                existingRappelActivite.Time = rappelActivite.Time;
                existingRappelActivite.TimeFrame = rappelActivite.TimeFrame;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("Aucun rappel trouvé avec cet ID.");
                }
                catch (Exception)
                {
                    return StatusCode(500, "Une erreur est survenue lors de la mise à jour du rappel.");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRappelActiviteAsync(RappelActivite rappelActivite)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Assigne le rappel à l'utilisateur
                rappelActivite.UtilisateurId = (int)etudiantId;

                // Ajoute le rappel
                await _context.RappelsActivite.AddAsync(rappelActivite);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRappelActivite", new { id = rappelActivite.Id }, rappelActivite);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout du rappel.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRappelActiviteAsync(int id)
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.UtilisateurId == utilisateur.Id);
                var etudiantId = etudiant?.Id;

                if (etudiantId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Accède au ancien rappel
                var rappelActivite = await _context.RappelsActivite.FindAsync(id);
                if (rappelActivite == null)
                {
                    return NotFound("Aucun rappel trouvé avec cet ID.");
                }

                // Vérifie que c'est son rappel
                if(rappelActivite.UtilisateurId != etudiantId)
                {
                    return StatusCode(403, "Ce n'est pas votre rappel.");
                }

                // Supprime le rappel
                _context.RappelsActivite.Remove(rappelActivite);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression du rappel.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }
    }
}
