using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.Reponse;
using ProjetSynthese.Server.Services;
using System.Globalization;

namespace ProjetSynthese.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExerciceInstanceController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILatexService _latexService;
        private readonly UserManager<IdentityUser> _userManager;

        public ExerciceInstanceController(UserManager<IdentityUser> userManager, MyDbContext context, ILatexService latexService)
        {
            _context = context;
            _latexService = latexService;
            _userManager = userManager;
        }

        [HttpGet("exercice/{exerciceId}", Name = "GetOrGenerateExerciceInstance")]
        public async Task<ActionResult<ExerciceInstance>> GetOrGenerateExerciceInstanceAsync(int exerciceId, bool nouvelleInstance)
        {
            try
            {
                // Accède à l'id de l'utilisateur
                var utilisateur = await _userManager.GetUserAsync(User);
                var utilisateurId = utilisateur?.Id;

                if (utilisateurId == null)
                {
                    return StatusCode(403, "Veuillez vous connecter.");
                }

                // Vérifie si une exercice instance existe déjà
                var doesExerciceInstanceExist = await verifyExerciceInstanceExist(exerciceId, utilisateurId);

                if (!doesExerciceInstanceExist || nouvelleInstance)
                {
                    // Génère l'exercice instance
                    int generatedExerciceId = await generateExerciceInstance(exerciceId, utilisateurId);

                    if (generatedExerciceId < 0)
                    {
                        return StatusCode(500, "Erreur durant la création de l'exercice instancié.");
                    }
                }

                // Accède à l'exercice instance
                var exerciceInstance = await _context.ExercicesInstance
                    .Include(e => e.VariablesInstance)
                    .Include(e => e.Exercice)
                    .Include(q => q.ReponsesUtilisateur)
                    .OrderByDescending(e => e.DateGenere)
                    .FirstOrDefaultAsync(e => e.ExerciceId == exerciceId && e.UtilisateurId == utilisateurId);

                if (exerciceInstance == null)
                {
                    return NotFound("Aucun exercice instancié trouvé.");
                }

                return Ok(exerciceInstance);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'exercice instancié.");
            }
        }


        /**
         * Vérifie si une exercice d'instance existe
         */
        private async Task<bool> verifyExerciceInstanceExist(int exerciceId, string utilisateurId)
        {
            var doesExerciceInstanceExist = await _context.ExercicesInstance
                    .AnyAsync(e => e.ExerciceId == exerciceId && e.UtilisateurId == utilisateurId);

            return doesExerciceInstanceExist;
        }

        /**
         * Génère une exercice pour l'utilisateur
         */
        private async Task<int> generateExerciceInstance(int exerciceId, string utilisateurId)
        {
            // Vérifie si la base de données prend en charge les transactions (pour les tests)
            var supportsTransactions = _context.Database.IsRelational();

            // Crée une transaction pour façilement rollback
            using var transaction = supportsTransactions ? await _context.Database.BeginTransactionAsync() : null;

            try
            {
                var exercice = await _context.Exercices
                    .Include(e => e.Variables)
                    .FirstOrDefaultAsync(e => e.Id == exerciceId);

                if (exercice == null)
                {
                    return -1;
                }

                // Crée le nouvelle exercice instancié
                ExerciceInstance newExerciceInstance = new ExerciceInstance();
                newExerciceInstance.ExerciceId = exercice.Id;
                newExerciceInstance.UtilisateurId = utilisateurId;
                newExerciceInstance.DateGenere = DateTime.Now;

                _context.ExercicesInstance.Add(newExerciceInstance);

                // Sauvegarde les changements
                await _context.SaveChangesAsync();

                // Génère les variables
                var variables = exercice.Variables ?? Enumerable.Empty<Variable>();

                foreach (var variable in variables)
                {
                    var resultat = await generateVariableInstance(variable, newExerciceInstance.Id);
                    if (!resultat)
                    {
                        if (transaction != null)
                        {
                            await transaction.RollbackAsync();
                        }
                        return -1;
                    }
                }

                if (transaction != null)
                {
                    await transaction.CommitAsync();
                }
                return newExerciceInstance.Id;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                return -1;
            }
        }

        /*
         * // A garder comme référence pour la refonte te de la fonction (-Xavier)
        private async Task<int> generateVariableInstance(Variable variable, int exerciceInstanceId)
        {
            Random random = new Random();
            float randomValue;

            VariableInstance newVariableInstance = new VariableInstance();
            newVariableInstance.VariableId = variable.Id;
            newVariableInstance.ExerciceInstanceId = exerciceInstanceId;

            // Récupere le nombre de chifre apres la virgule
            int incrementNbVirgule = BitConverter.GetBytes(decimal.GetBits((decimal)variable.Increment)[3])[2];

            if (variable.Min == variable.Max)
            {
                newVariableInstance.Valeur = variable.Min;
            }
            else if (variable.Max > variable.Min)
            {
                randomValue = (float)(random.NextDouble() * (variable.Max - variable.Min) + variable.Min);
                randomValue = (float)(Math.Round(randomValue / variable.Increment) * variable.Increment);
                newVariableInstance.Valeur = (float)Math.Round(randomValue, incrementNbVirgule);
            }
            else
            {
                randomValue = (float)(random.NextDouble() * (variable.Min - variable.Max) + variable.Max);
                randomValue = (float)(Math.Round(randomValue / variable.Increment) * variable.Increment);
                newVariableInstance.Valeur = (float)Math.Round(randomValue, incrementNbVirgule);
            }

            _context.VariablesInstance.Add(newVariableInstance);

            // Sauvegarde les changements
            try
            {
                await _context.SaveChangesAsync();
                return newVariableInstance.Id;

            }
            catch (Exception)
            {
                return -1;
            }
        }
         */

        /**
         * Génère une variable instancié
         */
        private async Task<bool> generateVariableInstance(Variable variable, int exerciceInstanceId)
        {
            try
            {
                // Charger les plages exclues pour cette variable
                var excludeRanges = await _context.ExcludeRanges
                    .Where(er => er.VariableId == variable.Id)
                    .ToListAsync();

                float randomValue;
                bool isValidValue = false;
                int maxAttempts = 100; // Éviter une boucle infinie et pour pas faire absolument exploser le serveur
                int attempts = 0;
                Random random = new Random();

                // Récupere le nombre de chifre apres la virgule
                int incrementNbVirgule = BitConverter.GetBytes(decimal.GetBits((decimal)variable.Increment)[3])[2];

                do
                {
                    if (variable.Min == variable.Max)
                    {
                        randomValue = variable.Min;
                    }
                    else if (variable.Max > variable.Min)
                    {
                        randomValue = (float)(random.NextDouble() * (variable.Max - variable.Min) + variable.Min);
                        randomValue = (float)(Math.Round(randomValue / variable.Increment) * variable.Increment);
                        randomValue = (float)Math.Round(randomValue, incrementNbVirgule);
                    }
                    else
                    {
                        randomValue = (float)(random.NextDouble() * (variable.Min - variable.Max) + variable.Max);
                        randomValue = (float)(Math.Round(randomValue / variable.Increment) * variable.Increment);
                        randomValue = (float)Math.Round(randomValue, incrementNbVirgule);
                    }

                    // Par défaut, on considère la valeur comme valide
                    isValidValue = true;

                    // Vérifier si la valeur est dans une plage exclue
                    if (excludeRanges != null && excludeRanges.Any())
                    {
                        foreach (var range in excludeRanges)
                        {
                            if (randomValue >= range.Start && randomValue <= range.End)
                            {
                                isValidValue = false;
                                break;
                            }
                        }
                    }

                    attempts++;
                } while (!isValidValue && attempts < maxAttempts);

                // Si on n'a pas trouvé de valeur valide après maxAttempts essais
                if (!isValidValue)
                {
                    Console.WriteLine($"Impossible de générer une valeur valide pour la variable {variable.Nom} après {maxAttempts} tentatives");
                    return false;
                }

                // Crée l'objet dans la db
                VariableInstance newVariableInstance = new VariableInstance
                {
                    ExerciceInstanceId = exerciceInstanceId,
                    VariableId = variable.Id,
                    Valeur = randomValue
                };

                _context.VariablesInstance.Add(newVariableInstance);

                // Sauvegarde les changements
                await _context.SaveChangesAsync();

                // Log de la valeur générée (pour le débogage)
                Console.WriteLine($"Variable {variable.Nom} générée avec la valeur {randomValue}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la génération de la variable {variable.Nom}: {ex.Message}");
                return false;
            }
        }

        /**
         * Retourne les reponse de l'utilisateur d'une question
         */
        [HttpGet("{exerciceInstanceId}/{questionId}", Name = "GetMostRecentReponsesUtilisateur")]
        public async Task<ActionResult<List<ReponseBase>>> GetMostRecentReponsesUtilisateur(int exerciceInstanceId, int questionId)
        {
            try
            {
                // Retourne les réponses de l'utilisateur les plus récente
                var reponsesUtilisateur = await _context.ReponsesUtilisateur
                    .Where(q => q.ExerciceInstanceId == exerciceInstanceId
                             && q.QuestionId == questionId
                             && q.DateRepondu == _context.ReponsesUtilisateur
                                 .Where(r => r.ExerciceInstanceId == exerciceInstanceId
                                          && r.QuestionId == questionId)
                                 .Max(r => r.DateRepondu))
                    .ToListAsync();

                return Ok(reponsesUtilisateur);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la question instancié.");
            }
        }


        /**
        * Répond à une question
        */
        [HttpPost("repondre/{exerciceInstanceId}/{questionId}", Name = "RepondreQuestion")]
        public async Task<ActionResult<List<ReponseBase>>> RepondreQuestion(int exerciceInstanceId, int questionId, [FromBody] List<string> reponses)
        {
            try
            {
                // TODO décomenter quand le front-end aura la persistence dans l'affichage
                // Vérifie que l'étudiant n'a pas déjà répondu à la question
                //var alreadyAnswered = await _context.ReponsesUtilisateur.AnyAsync(q => q.ExerciceInstanceId == exerciceInstanceId && q.QuestionId == questionId);

                //if (alreadyAnswered)
                //{
                //    return StatusCode(409, "La question à déjà été répondu.");
                //}

                // Accède à l'utilisateur connecté
                var utilisateur = await _userManager.GetUserAsync(User);
                if (utilisateur == null)
                {
                    return Unauthorized("Utilisateur non authentifié.");
                }

                // Accède à l'exercice instance
                var exerciceInstance = await _context.ExercicesInstance
                    .FirstOrDefaultAsync(ei => ei.Id == exerciceInstanceId);

                if (exerciceInstance == null)
                {
                    return NotFound("Exercice instance non trouvée.");
                }

                // Vérifie si l'exercice instance lui appartient
                if (exerciceInstance.UtilisateurId != utilisateur.Id)
                {
                    return Forbid("Vous n'êtes pas autorisé à accéder aux réponses de cet exercice.");
                }

                // Ajoute les réponse
                if (reponses != null && reponses.Any())
                {
                    // Calcul la date pour que c'est la même sur toutes les réponses
                    DateTime dateReponse = DateTime.Now;

                    // Ajoute les réponses de l'utilisateur
                    for (int i = 0; i < reponses.Count; i++)
                    {
                        var reponse = reponses[i];

                        // Calcul la réponse
                        var isCorrect = await isAnswer(exerciceInstanceId, questionId, reponse, i);

                        // N'ajoute pas la réponse si null
                        if (isCorrect == null)
                        {
                            continue;
                        }

                        // Ajoute la réponse à la liste
                        var reponseUtilisateur = new ReponseUtilisateur
                        {
                            ExerciceInstanceId = exerciceInstanceId,
                            QuestionId = questionId,
                            Position = i,
                            Valeur = reponse,
                            IsCorrect = (bool)isCorrect,
                            DateRepondu = dateReponse,
                        };

                        _context.ReponsesUtilisateur.Add(reponseUtilisateur);
                    }
                }
                else
                {
                    return StatusCode(400, "Aucune réponse fournie.");
                }

                // Sauvegarde les changements
                await _context.SaveChangesAsync();

                // Renvoie les réponses de l'utilisateur
                var reponsesUtilisateur = await _context.ReponsesUtilisateur
                    .Where(r => r.ExerciceInstanceId == exerciceInstanceId && r.QuestionId == questionId)
                    .OrderByDescending(r => r.DateRepondu)
                    .ToListAsync();

                return Ok(reponsesUtilisateur);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la question instancié.");
            }
        }

        /**
         * Vérifie si la réponse de l'utilisateur est bonne
         */
        private async Task<bool?> isAnswer(int exerciceInstanceId, int questionId, string reponseUtilisateurStr, int position)
        {
            try
            {
                // Accède à la question
                var question = await _context.Questions
                    .Include(q => q.Reponses)
                    .FirstOrDefaultAsync(q => q.Id == questionId);

                // Vérifie que la question existe et à une ou des réponses
                if (question?.Reponses == null)
                {
                    return null;
                }

                // Loop a travert les réponses pour trouvé si une match
                foreach (var reponse in question.Reponses)
                {
                    if (reponse is ReponseChoix)
                    {
                        if (isAnswerQuestionChoix(reponse as ReponseChoix, reponseUtilisateurStr))
                        {
                            return true;
                        }
                    }
                    else if (reponse is ReponseDeroulante)
                    {
                        if (isAnswerQuestionDeroulante(reponse as ReponseDeroulante, reponseUtilisateurStr, position))
                        {
                            return true;
                        }
                    }
                    else if (reponse is ReponseNumerique)
                    {
                        if (await isAnswerQuestionNumerique(reponse as ReponseNumerique, reponseUtilisateurStr, exerciceInstanceId))
                        {
                            return true;
                        }
                    }
                    else if (reponse is ReponseTroue)
                    {
                        if(isAnswerQuestionTroue(reponse as ReponseTroue, reponseUtilisateurStr, position))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /**
         * Vérifie la réponse d'une reponse possible à une question troué
         */
        private bool isAnswerQuestionTroue(ReponseTroue? reponse, string reponseUtilisateurStr, int position)
        {
            if (reponse?.PositionTexte == position && reponseUtilisateurStr == reponse.Valeur)
            {
                return true;
            }
            return false;
        }

        /**
         * Vérifie la réponse d'une reponse possible à une question à choix
         */
        private bool isAnswerQuestionChoix(ReponseChoix? reponse, string reponseUtilisateurStr)
        {
            if (reponse?.IsAnswer == true && reponseUtilisateurStr == reponse.Valeur)
            {
                return true;
            }
            return false;
        }

        /**
         * Vérifie la réponse d'une reponse possible à une question déroulante
         */
        private bool isAnswerQuestionDeroulante(ReponseDeroulante? reponse, string reponseUtilisateurStr, int position)
        {
            if (reponse?.PositionTexte == position && reponse?.IsAnswer == true && reponseUtilisateurStr == reponse.Valeur)
            {
                return true;
            }
            return false;
        }

        /**
         * Vérifie la réponse d'une reponse possible à une question numérique
         */
        private async Task<bool> isAnswerQuestionNumerique(ReponseNumerique? reponse, string reponseUtilisateurStr, int exerciceInstanceId)
        {
            float valeurReponse;

            // Vérifie si la réponse doit être calculé
            if (reponse?.IsCalculated == true)
            {
                // La réponse doit être calculé
                // Accède au variable instance de l'exercice
                var variablesInstance = await _context.VariablesInstance
                    .Where(v => v.ExerciceInstanceId == exerciceInstanceId)
                    .Include(v => v.Variable)
                    .ToListAsync();

                // Met les valeurs dans 2 liste
                var listeSymboles = new List<string>();
                var listeValeur = new List<float>();
                foreach (var variableInstance in variablesInstance)
                {
                    // Vérifie que la variable instancié pointe à une variable
                    if (variableInstance.Variable != null)
                    {
                        listeSymboles.Add(variableInstance.Variable.Nom);
                        listeValeur.Add(variableInstance.Valeur);
                    }
                }

                // Calcul la réponse
                string reponseCalculStr = await _latexService.CalculateLatex(reponse.Valeur, listeSymboles, listeValeur);

                // Convertie la réponse en float
                var isSuccess = float.TryParse(reponseCalculStr, NumberStyles.Float, CultureInfo.InvariantCulture, out valeurReponse);

                // Vérifie si il y a une erreur durant la conversion
                if (isSuccess == false)
                {
                    return false;
                }
            }
            else
            {
                // La réponse ne doit pas être calculé
                // Convertie la réponse float
                var isSuccess = float.TryParse(reponse?.Valeur, NumberStyles.Float, CultureInfo.InvariantCulture, out valeurReponse);

                // Vérifie si il y a une erreur durant la conversion
                if (isSuccess == false)
                {
                    return false;
                }
            }

            // Définit chiffre après la virgule et la marge d'erreur
            int chiffreApresLaVirgule = reponse?.ChiffreApresVirgule ?? 0;
            float margeErreur = (float)Math.Pow(10, -chiffreApresLaVirgule);

            // Convertie l'entré de l'utilisateur en float
            var isSuccessUserInputConvert = float.TryParse(reponseUtilisateurStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float reponseUtilisateurFloat);

            // Vérifie si il y a une erreur durant la conversion
            if (isSuccessUserInputConvert == false)
            {
                return false;
            }

            // TODO enlever quand le calcul 100% certain
            // Affiche la réponse dans la console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("La réponse est " + valeurReponse + " et la marge d'erreur est de " + margeErreur);
            Console.ResetColor();

            // Vérifier si les deux résultats sont égaux avec la marge d'erreur
            if (Math.Abs(valeurReponse - reponseUtilisateurFloat) < margeErreur)
            {
                // Les valeurs sont dans la marge d'erreur
                return true;
            }
            else
            {
                // Les valeurs sont différentes
                return false;
            }
        }
    }
}
