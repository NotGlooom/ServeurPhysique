using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetSynthese.Server.Services;

namespace ProjetSynthese.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Etudiant")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly UserManager<IdentityUser> _userManager;

        public StatusController(UserManager<IdentityUser> userManager, IStatusService statusService)
        {
            _statusService = statusService;
            _userManager = userManager;
        }

        // Envoie le status d'une activité et ses exercices
        [HttpGet("{activiteId}/statusActivite", Name = "GetStatusActivite")]
        public async Task<IActionResult> GetActiviteStatusAsync(int activiteId)
        {
            try {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var utilisateurId = utilisateur?.Id;

                if (utilisateurId == null)
                {
                    return StatusCode(403, "Veuillez-vous conneter.");
                }

                // Génère le status
                var statusActivite = await _statusService.GenerateStatusActiviteAsync(activiteId, utilisateurId);

                if (statusActivite == null)
                {
                    return NotFound("Activité pas trouvée.");
                }

                return Ok(statusActivite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de l'accès au status de l'activité.", Error = ex.Message });
            }
        }

        // Retourne le status des activité d'un utilisateur
        [HttpGet("statusActivites", Name = "GetStatusActivites")]
        public async Task<IActionResult> GetActivitesStatusAsync()
        {
            try {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var utilisateurId = utilisateur?.Id;

                if (utilisateurId == null)
                {
                    return StatusCode(403, "Veuillez-vous conneter.");
                }

                // Génère le status
                var statusActivites = await _statusService.GenerateStatusActivitesAsync(utilisateurId);
                return Ok(statusActivites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de l'accès au status des activités.", Error = ex.Message });
            }
        }

        // Envoie les status d'exercice
        [HttpGet("{exerciceId}/statusExercice", Name = "GetStatusExercice")]
        public async Task<IActionResult> GetExerciceStatusAsync(int exerciceId)
        {
            try {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var utilisateurId = utilisateur?.Id;

                if (utilisateurId == null)
                {
                    return StatusCode(403, "Veuillez-vous conneter.");
                }

                // Génère le status
                var statusExercice = await _statusService.GenerateStatusExerciceAsync(exerciceId, utilisateurId);

                if (statusExercice == null)
                {
                    return NotFound("Exercice pas trouvée.");
                }

                return Ok(statusExercice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de l'accès au status de l'exercice.", Error = ex.Message });
            }
        }

        [HttpGet("{activiteId}/exerciseCounts", Name = "GetExerciseCountsByStatus")]
        public async Task<IActionResult> GetExerciseCountsByStatusAsync(int activiteId)
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var utilisateurId = utilisateur?.Id;

                if (utilisateurId == null)
                {
                    return StatusCode(403, "Veuillez-vous conneter.");
                }

                // Trouve le nombre d'exercice fait par status
                var counts = await _statusService.GetExerciseCountsByStatusAsync(activiteId, utilisateurId);

                if (counts == null)
                {
                    return NotFound(new { Message = $"Aucune donnée de statut trouvée pour l'activité avec l'ID {activiteId}." });
                }

                (int nonCommence, int enCours, int termine) = counts.Value;
                return Ok(new { NonCommence = nonCommence, EnCours = enCours, Termine = termine });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération des comptes d'exercices par statut pour l'activité.", Error = ex.Message });
            }
        }

        [HttpGet("totalExerciseCounts", Name = "GetTotalExerciseCountsByStatus")]
        public async Task<IActionResult> GetTotalExerciseCountsByStatusAsync()
        {
            try
            {
                var totalCounts = await _statusService.GetTotalExerciseCountsByStatusAsync();

                if (totalCounts == null)
                {
                    return NotFound(new { Message = "Aucune donnée de statut trouvée pour les exercices." });
                }

                (int totalNonCommence, int totalEnCours, int totalTermine) = totalCounts.Value;
                return Ok(new { TotalNonCommence = totalNonCommence, TotalEnCours = totalEnCours, TotalTermine = totalTermine });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération des comptes totaux d'exercices par statut.", Error = ex.Message });
            }
        }
    }
}
