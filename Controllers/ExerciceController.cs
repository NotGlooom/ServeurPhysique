using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.DTOs;

namespace ProjetSynthese.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciceController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageExtractionService _imageExtractionService;

        /// <summary>
        /// Initialise une nouvelle instance du contrôleur ExerciceController.
        /// </summary>
        /// <param name="context">Le contexte de base de données</param>
        /// <param name="mapper">Le service de mapping AutoMapper</param>
        public ExerciceController(MyDbContext context, IMapper mapper, IImageExtractionService imageExtractionService)
        {
            _context = context;
            _mapper = mapper;
            _imageExtractionService = imageExtractionService;
        }

        /// <summary>
        /// Récupère la liste complète des exercices avec leurs relations associées.
        /// </summary>
        /// <returns>
        /// 200 OK avec la liste des exercices mappés en DTO
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciceDto>>> GetListExercicesAsync()
        {
            try
            {
                var exercices = await _context.Exercices
                    .Include(e => e.Auteur)
                    .Include(e => e.Variables)
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.Reponses)
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.Indices)
                    .Include(e => e.Activite)
                    .ToListAsync();

                if (!exercices.Any())
                {
                    return Ok(new List<ExerciceDto>());
                }
                var exerciceDto = _mapper.Map<List<ExerciceDto>>(exercices);
                return Ok(exerciceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des exercices. " + ex.Message);
            }
        }


        /// <summary>
        /// Récupère un exercice spécifique par son identifiant avec toutes ses relations.
        /// </summary>
        /// <param name="id">L'identifiant de l'exercice à récupérer</param>
        /// <returns>
        /// 200 OK avec l'exercice mappé en DTO
        /// 200 OK avec message si aucun exercice n'est trouvé
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpGet("{id:int}", Name = "GetExercice")]
        public async Task<IActionResult> GetExerciceAsync(int id)
        {
            try
            {
                var exercice = await _context.Exercices
                    .Include(e => e.Auteur)
                    .Include(e => e.Variables)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Reponses)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Indices)
                    .Include(e => e.Activite)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exercice == null)
                {
                    return Ok("Aucun exercice trouvé.");
                }

                var exerciceDto = _mapper.Map<ExerciceDto>(exercice);
                return Ok(exerciceDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'exercice.");
            }
        }


        /// <summary>
        /// Récupère tous les exercices créés par un enseignant spécifique.
        /// </summary>
        /// <param name="enseignantId">L'identifiant de l'enseignant</param>
        /// <returns>
        /// 200 OK avec la liste des exercices de l'enseignant
        /// 200 OK avec message si aucun exercice n'est trouvé
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpGet("auteur/{enseignantId}")]
        public async Task<IActionResult> GetListExerciceByAutheur(string enseignantId)
        {
            try
            {
                var exercices = await _context.Exercices
                    .Include(e => e.Auteur)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Reponses)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Indices)
                    .Include(e => e.Variables)
                    .Include(e => e.Activite)
                    .Where(e => e.AuteurUtilisateurId == enseignantId)
                    .ToListAsync();

                if (!exercices.Any())
                {
                    return Ok(new List<ExerciceDto>());
                }

                var exerciceDtos = _mapper.Map<List<ExerciceDto>>(exercices);
                return Ok(exerciceDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des exercices.");
            }
        }

        /// <summary>
        /// Récupère tous les exercices selon leur statut public ou privé.
        /// </summary>
        /// <param name="status">True pour les exercices publics, False pour les privés</param>
        /// <returns>
        /// 200 OK avec la liste des exercices filtrés par statut
        /// 200 OK avec message si aucun exercice n'est trouvé
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpGet("publique/{status:bool}")]
        public async Task<IActionResult> GetAllPubliqueExercice(bool status)
        {
            try
            {
                var exercices = await _context.Exercices
                    .Include(e => e.Auteur)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Reponses)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Indices)
                    .Include(e => e.Variables)
                    .Include(e => e.Activite)
                    .Where(e => e.IsPublique == status)
                    .ToListAsync();

                if (!exercices.Any())
                {
                    return Ok(new List<ExerciceDto>());
                }

                var exerciceDtos = _mapper.Map<List<ExerciceDto>>(exercices);
                return Ok(exerciceDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des exercices.");
            }
        }


        /// <summary>
        /// Modifie un exercice existant et ses questions associées.
        /// </summary>
        /// <param name="id">L'identifiant de l'exercice à modifier</param>
        /// <param name="request">Les nouvelles données de l'exercice et ses questions</param>
        /// <returns>
        /// 200 OK avec l'exercice modifié
        /// 400 Bad Request si l'ID ne correspond pas
        /// 409 Conflict en cas d'erreur de concurrence
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Enseignant")]
        public async Task<IActionResult> ModifierExerciceAsync(int id, ExerciceQuestionDto request)
        {
            var exercice = request.Exercice;
            var questions = request.ListeQuestions;

            // Valide que l'ID correspond
            if (id != exercice.Id)
            {
                return BadRequest("L'ID de l'exercice ne correspond pas.");
            }

            try
            {
                var exerciceExistant = await _context.Exercices
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Reponses)
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Indices)
                    .Include(e => e.Variables)
                    .Include(e => e.Activite)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (exerciceExistant == null)
                {
                    return Ok("Aucun exercice trouvé avec cet ID.");
                }

                // Modifier les Questions
                if (questions != null)
                {
                    if (exerciceExistant.Questions.Count > 0 && questions.Count > 0)
                    {
                        // Verifier si la longueur des questions envoyer est plus courte
                        if (exerciceExistant.Questions.Count > questions.Count)
                        {
                            // Find questions to remove from exerciceExistant.Questions
                            var questionsToRemove = exerciceExistant.Questions
                                .Where(q => !questions.Any(q2 => q2.Id == q.Id))
                                .ToList();

                            foreach (var question in questionsToRemove)
                            {
                                exerciceExistant.Questions.Remove(question);
                            }
                        } else
                        {
                            // modifier les questions existante
                            for (int i = 0; i < questions.Count; i++)
                            {
                                if (i < exerciceExistant.Questions.Count)
                                {
                                    questions[i].Enonce = await _imageExtractionService.ExtractImages(questions[i].Enonce);
                                    questions[i].Id = exerciceExistant.Questions[i].Id;

                                    if (questions[i].Indices != null && questions[i].Indices.Count > 0)
                                    {

                                        // validation des indices
                                        foreach (var indice in questions[i].Indices)
                                        {
                                            if (string.IsNullOrWhiteSpace(indice.IndiceText))
                                            {
                                                return BadRequest("Un indice contient un texte vide ou null.");
                                            }

                                            if (indice.Essaies <= 0)
                                            {
                                                return BadRequest("Le nombre d'essais pour un indice doit être supérieur à zéro.");
                                            }

                                            indice.Id = 0; // Reset indice ID
                                        }
                                    }

                                    for (int y = 0; y < questions[i].Reponses.Count; y++)
                                    {

                                        if (y < exerciceExistant.Questions[i]?.Reponses.Count)
                                        {
                                            questions[i].Reponses[y].Id = exerciceExistant.Questions[i].Reponses[y].Id;
                                        }
                                        else
                                        {
                                            questions[i].Reponses[y].Id = 0;
                                        }
                                    }

                                    exerciceExistant.Questions[i] = questions[i];
                                }
                                else
                                {
                                    questions[i].Id = 0;
                                    foreach (var reponse in questions[i].Reponses)
                                    {
                                        reponse.Id = 0;
                                    }
                                    exerciceExistant.Questions.Add(questions[i]);
                                }
                            }
                        }
                    } else if (exerciceExistant.Questions.Count <= 0 && questions.Count > 0)
                    {
                        // loop a travers la nouvelle liste de questions recu
                        foreach (var question in questions)
                        {
                            question.Id = 0;

                            foreach (var reponse in question.Reponses)
                            {
                                reponse.Id = 0;
                            }
                        }

                        // ajouter les nouvelles questions a liste
                        exerciceExistant.Questions = questions;
                    } else if (exerciceExistant.Questions.Count > 0 && questions.Count <= 0)
                    {
                        // vider les questions dans l'exercice existant
                        exerciceExistant.Questions = [];
                    }
                }

                // Extrait les images
                var enonceImageExtrait = await _imageExtractionService.ExtractImages(exercice.Enonce);

                // Modifier les valeurs
                exerciceExistant.Titre = exercice.Titre;
                exerciceExistant.Enonce = enonceImageExtrait;
                exerciceExistant.Variables = exercice.Variables;
                exerciceExistant.IsPublique = exercice.IsPublique;
                exerciceExistant.DemarcheDisponible = exercice.DemarcheDisponible;
                exerciceExistant.Image = exercice.Image;

                // retirer les instances de l'ancien exercice
                var instances = await _context.ExercicesInstance
                    .Where(ei => ei.ExerciceId == id)
                    .ToListAsync();
                _context.ExercicesInstance.RemoveRange(instances);

                await _context.SaveChangesAsync();

                return Ok(exerciceExistant);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExerciceExistsAsync(id))
                {
                    return Ok("Aucun exercice trouvé avec cet ID.");
                }
                return StatusCode(409, "Une erreur de concurrence s'est produite lors de la mise à jour de l'exercice.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'exercice.");
            }
        }

        /// <summary>
        /// Vérifie si un exercice existe dans la base de données.
        /// </summary>
        /// <param name="id">L'identifiant de l'exercice à vérifier</param>
        /// <returns>True si l'exercice existe, False sinon</returns>
        private async Task<bool> ExerciceExistsAsync(int id)
        {
            return await _context.Exercices.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Crée un nouvel exercice avec ses questions associées.
        /// </summary>
        /// <param name="request">Les données de l'exercice et ses questions</param>
        /// <returns>
        /// 200 OK avec l'exercice créé
        /// 400 Bad Request si les données sont invalides
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpPost]
        [Authorize(Roles = "Enseignant")]
        public async Task<IActionResult> AjouterExerciceAsync(ExerciceQuestionDto request)
        {
            var exercice = request.Exercice;
            if (exercice == null || !ModelState.IsValid) // Ajoute une validation de l'entrée
            {
                return BadRequest("Les données de l'exercice sont invalides.");
            }

            try
            {
                var activite = await _context.Activities.FindAsync(exercice.ActiviteId);
                if (activite == null)
                {
                    return BadRequest("L'activité spécifiée n'existe pas.");
                }

                if (request.ListeQuestions != null)
                {
                    // Retirer tout les identifiant temporaire ajouter dans le frontend
                    foreach(var question in request.ListeQuestions)
                    {
                        // Retirer les id des questions
                        question.Id = 0;

                        question.Enonce = await _imageExtractionService.ExtractImages(question.Enonce);

                        // Validation des indices
                        if (question.Indices != null && question.Indices.Count > 0)
                        {
                            foreach (var indice in question.Indices)
                            {
                                if (string.IsNullOrWhiteSpace(indice.IndiceText))
                                {
                                    return BadRequest("Un indice contient un texte vide ou null.");
                                }

                                if (indice.Essaies <= 0)
                                {
                                    return BadRequest("Le nombre d'essais pour un indice doit être supérieur à zéro.");
                                }

                                indice.Id = 0; // Reset indice ID
                            }
                        }


                        if (question.Reponses != null || question.Reponses.Count > 0)
                        {
                            foreach (var reponse in question.Reponses)
                            {
                                // Retirer le id des reponses
                                reponse.Id = 0;


                            }
                        }
                    }

                    // Extrait les images
                    var enonceImageExtrait = await _imageExtractionService.ExtractImages(exercice.Enonce);

                    // Creer l'objet exercice avec les questions
                    var nouvExercice = new Exercice
                    {
                        Titre = exercice.Titre,
                        Enonce = enonceImageExtrait,
                        DemarcheDisponible = exercice.DemarcheDisponible,
                        ActiviteId = exercice.ActiviteId,
                        AuteurUtilisateurId = exercice.AuteurUtilisateurId,
                        IsPublique = exercice.IsPublique,
                        Variables = exercice.Variables,
                        Questions = request.ListeQuestions
                    };

                    // Ajouter a la DB
                    _context.Exercices.Add(nouvExercice);
                    await _context.SaveChangesAsync();
                    return Ok(nouvExercice); // retourner l'exercice avec code 200
                } 
                else
                {
                    // Ajouter l'exercice a la DB
                    _context.Exercices.Add(exercice);
                    await _context.SaveChangesAsync();
                    return Ok(exercice); // retourner l'exercice avec code 200
                }
                
                
            }
            catch (Exception e)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'exercice. Détails : " + e.Message + e.InnerException);
            }
        }

        /// <summary>
        /// Supprime un exercice et ses instances associées.
        /// </summary>
        /// <param name="id">L'identifiant de l'exercice à supprimer</param>
        /// <returns>
        /// 204 NoContent si la suppression est réussie
        /// 200 OK avec message si l'exercice n'est pas trouvé
        /// 500 Internal Server Error en cas d'erreur serveur
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Enseignant")]
        public async Task<IActionResult> SupprimerExerciceAsync(int id)
        {
            try
            {
                // retirer les instances
                var instances = await _context.ExercicesInstance
                    .Where(ei => ei.ExerciceId == id)
                    .ToListAsync();
                _context.ExercicesInstance.RemoveRange(instances);
                await _context.SaveChangesAsync();

                // supprimer l'exercice
                var exercice = await _context.Exercices.FindAsync(id);
                if (exercice == null)
                {
                    return Ok("Aucun exercice trouvé avec cet ID.");
                }

                // Suppression de l'exercice
                _context.Exercices.Remove(exercice);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression de l'exercice.");
            }
        }

        /// <summary>
        /// Associe un exercice à une activité spécifique.
        /// </summary>
        /// <param name="exerciceId">L'identifiant de l'exercice</param>
        /// <param name="activiteId">L'identifiant de l'activité</param>
        /// <returns>
        /// 200 OK avec message de confirmation
        /// 200 OK avec message si l'exercice ou l'activité n'est pas trouvé
        /// </returns>
        [HttpPost("{exerciceId}/lier-activite/{activiteId}")]
        public async Task<IActionResult> LierExerciceToActivite(int exerciceId, int activiteId)
        {
            var exercice = await _context.Exercices.FindAsync(exerciceId);
            var activite = await _context.Activities.FindAsync(activiteId);

            if (exercice == null || activite == null)
            {
                return Ok("Exercice ou activité non trouvé.");
            }

            // Vérifie si la collection Exercices est null
            if (activite.Exercices == null)
            {
                activite.Exercices = new List<Exercice>();
            }

            // Ajoute l'exercice à l'activité
            activite.Exercices.Add(exercice);
            await _context.SaveChangesAsync();

            return Ok("Exercice lié à l'activité.");
        }

        /// <summary>
        /// Dissocie un exercice d'une activité.
        /// </summary>
        /// <param name="exerciceId">L'identifiant de l'exercice</param>
        /// <param name="activiteId">L'identifiant de l'activité</param>
        /// <returns>
        /// 200 OK avec message de confirmation
        /// 200 OK avec message si l'exercice ou l'activité n'est pas trouvé
        /// </returns>
        [HttpPost("{exerciceId}/dissocier-activite/{activiteId}")]
        public async Task<IActionResult> DissocierExerciceFromActivite(int exerciceId, int activiteId)
        {
            var exercice = await _context.Exercices.FindAsync(exerciceId);
            var activite = await _context.Activities
                .Include(a => a.Exercices)
                .FirstOrDefaultAsync(a => a.Id == activiteId);

            if (exercice == null || activite == null)
            {
                return Ok("Exercice ou activité non trouvé.");
            }

            // Vérifie si la collection Exercices n'est pas null avant d'essayer de supprimer
            if (activite.Exercices != null && activite.Exercices.Contains(exercice))
            {
                activite.Exercices.Remove(exercice);
            }

            await _context.SaveChangesAsync();

            return Ok("Exercice dissocié de l'activité.");
        }
    }
}
