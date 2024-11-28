using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.DTOs;
using ProjetSynthese.Server.Models.Question;
using System.Diagnostics;

namespace ProjetSynthese.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiviteController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ActiviteController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Récupère toutes les activités avec leurs exercices, questions et auteur.
        /// </summary>
        /// <returns>Renvoie une liste de toutes les activités sous forme d'ActiviteDto.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActiviteDto>>> GetActivitesAsync()
        {
            try
            {
                var activites = await _context.Activities
                    .Include(a => a.GroupeCours)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .Include(a => a.Auteur)
                    .ToListAsync();

                var activiteDtos = _mapper.Map<IEnumerable<ActiviteDto>>(activites);
                return Ok(activiteDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des activités.");
            }
        }

        /// <summary>
        /// Récupère une activité spécifique par son ID avec ses exercices, questions, indices et auteur.
        /// </summary>
        /// <param name="id">L'ID de l'activité à récupérer.</param>
        /// <returns>Renvoie l'activité demandée sous forme d'ActiviteDto.</returns>
        [HttpGet("{id}", Name = "GetActivite")]
        public async Task<ActionResult<ActiviteDto>> GetActiviteAsync(int id)
        {
            try
            {
                var activite = await _context.Activities
                    .Include(a => a.Auteur)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Indices)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (activite == null)
                {
                    return Ok("Aucune activité trouvée.");
                }

                var activiteDto = _mapper.Map<ActiviteDto>(activite);
                return Ok(activiteDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'activité. " + e.Message);
            }
        }

        /// <summary>
        /// Récupère les activités par l'ID de l'auteur.
        /// </summary>
        /// <param name="enseignantId">L'ID de l'auteur (enseignant) des activités.</param>
        /// <returns>Renvoie une liste d'activités liées à l'auteur sous forme d'ActiviteDto.</returns>
        [HttpGet("auteur/{enseignantId}")]
        public async Task<ActionResult<IEnumerable<ActiviteDto>>> GetActiviteByAuteur(string enseignantId)
        {
            try
            {
                var activites = await _context.Activities
                    .Include(a => a.Auteur)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Indices)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .Where(a => a.AuteurUtilisateurId == enseignantId)
                    .ToListAsync();

                if (!activites.Any())
                {
                    return Ok("Aucune activité trouvée.");
                }

                var activiteDtos = _mapper.Map<IEnumerable<ActiviteDto>>(activites);
                return Ok(activiteDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la récupération des activités: {ex.Message}");
            }
        }


        /// <summary>
        /// Récupère toutes les activités en fonction de leur statut de publication (publique ou privée).
        /// </summary>
        /// <param name="status">Statut de publication des activités (true pour publique, false pour privée).</param>
        /// <returns>Renvoie une liste d'activités sous forme d'ActiviteDto.</returns>
        [HttpGet("publique/{status:bool}")]
        public async Task<ActionResult<IEnumerable<ActiviteDto>>> GetAllActivitePublique(bool status)
        {
            try
            {
                var activites = await _context.Activities
                    .Include(a => a.Auteur)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Indices)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .Where(a => a.IsPublique == status)
                    .ToListAsync();

                if (!activites.Any())
                {
                    return Ok("Aucune activité trouvée.");
                }

                var activiteDtos = _mapper.Map<IEnumerable<ActiviteDto>>(activites);
                return Ok(activiteDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la récupération des activités: {ex.Message}");
            }
        }


        /// <summary>
        /// Récupère les activités qui sont publiques ou dont l'auteur est l'enseignant spécifié.
        /// </summary>
        /// <param name="enseignantId">L'ID de l'auteur (enseignant). Pour l'instant, cela sera toujours 1.</param>
        /// <returns>Renvoie une liste d'activités sous forme d'ActiviteDto.</returns>
        [HttpGet("publique-ou-auteur/{enseignantId}")]
        public async Task<ActionResult<IEnumerable<ActiviteDto>>> GetActivitesPubliqueOuAuteur(string enseignantId)
        {
            try
            {
                // Get all public activities
                var activitesPubliques = await _context.Activities
                    .Where(a => a.IsPublique)
                    .Include(a => a.Auteur)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Indices)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .ToListAsync();

                // Get all activities authored by the specified enseignantId
                var activitesAuteur = await _context.Activities
                    .Where(a => a.AuteurUtilisateurId == enseignantId)
                    .Include(a => a.Auteur)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Reponses)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Questions)
                            .ThenInclude(q => q.Indices)
                    .Include(a => a.Exercices)
                        .ThenInclude(e => e.Variables)
                    .ToListAsync();

                // Combine and remove duplicates (using distinct based on activity ID)
                var activitesCombinees = activitesPubliques
                    .Union(activitesAuteur)
                    .GroupBy(a => a.Id) // Assuming Id is the unique identifier
                    .Select(g => g.First())
                    .ToList();

                if (activitesCombinees.Count == 0)
                {
                    return Ok("Aucune activité trouvée.");
                }

                var activitesDto = _mapper.Map<IEnumerable<ActiviteDto>>(activitesCombinees);
                return Ok(activitesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la récupération des activités: {ex.Message}");
            }
        }

        /// <summary>
        /// Modifie une activité existante.
        /// </summary>
        /// <param name="id">L'ID de l'activité à modifier.</param>
        /// <param name="activite">Les nouvelles données de l'activité.</param>
        /// <returns>Renvoie un code 204 No Content en cas de succès, ou une erreur en cas d'échec.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutActiviteAsync(int id, Activite activite)
        {
            var errors = new List<string>();

            // Vérifier si l'activité est null ou si l'id ne correspont pas
            if (activite == null || id != activite.Id)
            {
                errors.Add("Aucune activité n'a été fournie ou l'ID de l'activité est incorrect.");
            }

            // Tenter de récupérer l'activité se il n'y pas pas d'erreurs
            Activite existanteActivite = null;
            if (errors.Count == 0)
            {
                existanteActivite = await _context.Activities.FindAsync(id);
                if (existanteActivite == null)
                {
                    errors.Add("Aucune activité trouvée avec cet ID.");
                }
            }

            // D'autres validations
            if (errors.Count == 0)
            {
                if (string.IsNullOrWhiteSpace(activite.Nom))
                {
                    errors.Add("Le nom de l'activité est requis.");
                }

                if (string.IsNullOrWhiteSpace(activite.Description))
                {
                    errors.Add("La description est requise.");
                }

                if (activite.DateEcheance < activite.DatePublication)
                {
                    errors.Add("La date d'échéance ne doit pas être inférieure à la date de publication.");
                }
                else if (activite.DateEcheance == activite.DatePublication)
                {
                    // Si les dates sont identiques, vérifier les heures
                    if (activite.DateEcheance?.TimeOfDay < activite.DatePublication?.TimeOfDay)
                    {
                        errors.Add("L'heure d'échéance doit être postérieure à l'heure de publication lorsque les dates sont identiques.");
                    }
                }
            }

            // Retourner les erreurs si il y en a
            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            // Mise à jour des propriétés de l'activité
            existanteActivite.Nom = activite.Nom;
            existanteActivite.Description = activite.Description;
            existanteActivite.DateEcheance = activite.DateEcheance;
            existanteActivite.DatePublication = activite.DatePublication;
            existanteActivite.IsPublique = activite.IsPublique;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                if (!ActiviteExists(id))
                {
                    return Ok("Aucune activité trouvée avec cet ID.");
                }
                return StatusCode(500, "Une erreur de concurrence s'est produite: " + dbEx.Message + dbEx.InnerException);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'activité: " + ex.Message + ex.InnerException);
            }

            return NoContent();
        }

        /// <summary>
        /// Ajoute une nouvelle activité.
        /// </summary>
        /// <param name="activite">Les données de l'activité à ajouter.</param>
        /// <returns>Renvoie l'activité créée avec son ID.</returns>
        [HttpPost]
        public async Task<ActionResult<Activite>> PostActiviteAsync(Activite activite)
        {
            
            var errors = new List<string>();

            if (activite == null)
            {
                errors.Add("Aucune activité n'a été fournie.");
            }
            else
            {
                
                if (string.IsNullOrWhiteSpace(activite.Nom))
                {
                    errors.Add("Le nom de l'activité est requis.");
                }

                if (string.IsNullOrWhiteSpace(activite.Description))
                {
                    errors.Add("La description est requise.");
                }

                if (activite.DatePublication < DateTime.Now.AddDays(-1))
                {
                    errors.Add("La date de publication ne doit pas être inférieure à aujourd'hui.");
                }

                if (activite.DateEcheance < activite.DatePublication)
                {
                    errors.Add("La date d'échéance ne doit pas être inférieure à la date de publication.");
                }
                else if (activite.DateEcheance == activite.DatePublication)
                {
                    // Si les dates sont identiques, vérifier les heures
                    if (activite.DateEcheance?.TimeOfDay < activite.DatePublication?.TimeOfDay)
                    {
                        errors.Add("L'heure d'échéance doit être postérieure à l'heure de publication lorsque les dates sont identiques.");
                    }
                }
            }

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            _context.Activities.Add(activite);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'activité: " + e.Message + e.InnerException);
            }

            return CreatedAtAction("GetActivite", new { id = activite.Id }, activite);
        }

        /// <summary>
        /// Clone une activité existante en créant une nouvelle activité avec les mêmes propriétés,
        /// y compris ses exercices, questions, variables et rappels.
        /// </summary>
        /// <param name="activity">L'activité à cloner. Ne peut pas être nulle.</param>
        /// <returns>Une action qui représente le résultat de l'opération de clonage.</returns>
        [HttpPost("clone")]
        public async Task<IActionResult> CloneActivity([FromBody] Activite activity)
        {
            var errors = new List<string>();

            // Valider si l'activité fournie est null
            if (activity == null)
            {
                errors.Add("Aucune activité n'a été fournie.");
            }
            else // D'autres validations
            {
                if (string.IsNullOrWhiteSpace(activity.Nom))
                {
                    errors.Add("Le nom de l'activité est requis.");
                }

                if (string.IsNullOrWhiteSpace(activity.Description))
                {
                    errors.Add("La description est requise.");
                }

                if (activity.DatePublication < DateTime.Now.AddDays(-1))
                {
                    errors.Add("La date de publication ne doit pas être inférieure à aujourd'hui.");
                }

                if (activity.DateEcheance < activity.DatePublication)
                {
                    errors.Add("La date d'échéance ne doit pas être inférieure à la date de publication.");
                }
            }

            // Retourner les erreurs si il y en a
            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }


            // Crée un nouvel objet Activite sans ID
            var clonedActivity = new Activite
            {
                Nom = activity.Nom,
                Description = activity.Description,
                IsPublique = activity.IsPublique,
                DatePublication = activity.DatePublication,
                DateEcheance = activity.DateEcheance,
                GroupeCoursId = activity.GroupeCoursId,
                IsArchiver = activity.IsArchiver,
                AuteurUtilisateurId = activity.AuteurUtilisateurId, // Associer l'ID de l'auteur
                Auteur = null, // Cloner les détails de l'auteur si nécessaire
                RappelsActivite = activity.RappelsActivite?.Select(rappel => new RappelActivite
                {
                    UtilisateurId = rappel.UtilisateurId,
                    ActiviteId = 0, // Sera défini après l'enregistrement de l'activité clonée
                    Time = rappel.Time,
                    TimeFrame = rappel.TimeFrame,
                    Utilisateur = rappel.Utilisateur != null ? new Etudiant
                    {
                        Id = rappel.Utilisateur.Id,
                        numeroEtudiant = rappel.Utilisateur.numeroEtudiant,
                        Nom = rappel.Utilisateur.Nom,
                        Prenom = rappel.Utilisateur.Prenom
                    } : null,
                    Activite = rappel.Activite
                }).ToList()
            };

            // Cloner les exercices et leurs détails
            if (activity.Exercices != null)
            {
                clonedActivity.Exercices = activity.Exercices.Select(exercice => new Exercice
                {
                    Titre = exercice.Titre,
                    Enonce = exercice.Enonce,
                    Image = exercice.Image,
                    DemarcheDisponible = exercice.DemarcheDisponible,
                    IsPublique = exercice.IsPublique,
                    AuteurUtilisateurId = exercice.AuteurUtilisateurId,
                    ActiviteId = 0, // Sera définie plus tard après le post
                    Variables = exercice.Variables?.Select(variable => new Variable
                    {
                        Nom = variable.Nom,
                        Min = variable.Min,
                        Max = variable.Max,
                        Increment = variable.Increment,
                        Unite = variable.Unite,
                    }).ToList(),
                    Questions = exercice.Questions?.Select(question =>
                        (QuestionBase)question.Clone()
                    ).ToList()
                }).ToList();
            }

            _context.Activities.Add(clonedActivity);
            try
            {
                await _context.SaveChangesAsync(); // Enregistre l'activité et génère de nouveaux IDs pour les exercices
            }
            catch (DbUpdateException dbEx)
            {
                // Capture detailed error messages
                var innerEx = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return StatusCode(500, "Une erreur est survenue lors de l'enregistrement de l'activité : " + innerEx);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'enregistrement de l'activité : " + ex.Message);
            }

            // Après l'enregistrement, met à jour l'ActiviteId pour chaque exercice et l'ExerciceId pour chaque question
            foreach (var exercice in clonedActivity.Exercices)
            {
                exercice.ActiviteId = clonedActivity.Id; // Assigne l'ID de l'activité clonée à chaque exercice
                exercice.Questions?.ForEach(q => q.ExerciceId = exercice.Id); // Assigne l'ID de l'exercice à chaque question
            }

            // Enregistre les modifications pour les exercices et les questions
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivite", new { id = clonedActivity.Id }, clonedActivity);
        }



        /// <summary>
        /// Supprime une activité par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'activité à supprimer.</param>
        /// <returns>Renvoie un code 204 No Content en cas de succès.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActiviteAsync(int id)
        {
            var activity = await _context.Activities
                .Include(a => a.Exercices)
                    .ThenInclude(e => e.Questions)
                .Include(a => a.Exercices)
                    .ThenInclude(e => e.Variables)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound("Aucune activité trouvée avec cet ID.");
            }

            try
            {
                foreach (var exercice in activity.Exercices)
                {
                    foreach (var question in exercice.Questions)
                    {
                        var reponsesUtilisateur = _context.ReponsesUtilisateur
                            .Where(ru => ru.QuestionId == question.Id);
                        _context.ReponsesUtilisateur.RemoveRange(reponsesUtilisateur);
                    }

                    var exercicesInstance = _context.ExercicesInstance
                        .Where(ei => ei.ExerciceId == exercice.Id);
                    _context.ExercicesInstance.RemoveRange(exercicesInstance);

                    foreach (var variable in exercice.Variables)
                    {
                        var variablesInstance = _context.VariablesInstance
                            .Where(vi => vi.VariableId == variable.Id);
                        _context.VariablesInstance.RemoveRange(variablesInstance);
                    }
                }

                var exercicesToDelete = _context.Exercices.Where(e => e.ActiviteId == id).ToList();
                _context.Exercices.RemoveRange(exercicesToDelete);

                _context.Activities.Remove(activity);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la suppression de l'activité : {ex.Message}");
            }
        }



        /// <summary>
        /// Rendre une activité publique.
        /// </summary>
        /// <param name="id">L'ID de l'activité à rendre publique.</param>
        /// <returns>Renvoie un message confirmant le changement.</returns>
        [HttpPost("{id}/rendre-publique")]
        public async Task<IActionResult> RendreActivitePublique(int id)
        {
            var activite = await _context.Activities.FindAsync(id);
            if (activite == null)
            {
                return Ok(new { message = "Activité non trouvée." });
            }
            if (activite.IsPublique)
            {
                return BadRequest(new { message = "L'activité est déjà publique." });
            }
            activite.IsPublique = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Activité rendue publique avec succès." });
        }

        /// <summary>
        /// Rendre une activité privée.
        /// </summary>
        /// <param name="id">L'ID de l'activité à rendre privée.</param>
        /// <returns>Renvoie un message confirmant le changement.</returns>
        [HttpPost("{id}/rendre-privee")]
        public async Task<IActionResult> RendreActivitePrivee(int id)
        {
            var activite = await _context.Activities.FindAsync(id);

            if (activite == null)
            {
                return NotFound(new { message = "Activité non trouvée." });
            }

            if (!activite.IsPublique)
            {
                return BadRequest(new { message = "L'activité est déjà privée." });
            }

            activite.IsPublique = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Activité rendue privée avec succès." });
        }

        private bool ActiviteExists(int id)
        {
            return _context.Activities.Any(x => x.Id == id);
        }
    }
}
