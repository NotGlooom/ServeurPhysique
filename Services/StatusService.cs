using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Services
{
    public interface IStatusService
    {
        Task<StatusActivite?> GenerateStatusActiviteAsync(int activiteId, string utilisateurId);
        Task<List<StatusActivite>> GenerateStatusActivitesAsync(string utilisateurId);
        Task<StatusExercice> GenerateStatusExerciceAsync(int exerciceId, string utilisateurId);
        Task<(int nonCommence, int enCours, int termine)?> GetExerciseCountsByStatusAsync(int activiteId, string utilisateurId);

        Task<(int totalNonCommence, int totalEnCours, int totalTermine)?> GetTotalExerciseCountsByStatusAsync();
    }

    public class StatusService : IStatusService
    {
        private readonly MyDbContext _context;

        public StatusService(MyDbContext context)
        {
            _context = context;
        }

        /**
         * Génère et retourne le status d'une activité
         */
        public async Task<StatusActivite?> GenerateStatusActiviteAsync(int activiteId, string utilisateurId)
        {
            // TODO vérifié que l'étudiant à accès à cette activité
            // Vérifie que l'activité existe
            var activiteAvecExercices = await _context.Activities
                .Where(a => a.Id == activiteId)
                .Select(a => new
                {
                    ActiviteId = a.Id,
                    Exercices = a.Exercices.Select(e => e.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (activiteAvecExercices == null)
            {
                return null;
            }

            // Crée la variable de retour
            var statusActivite = new StatusActivite
            {
                ActiviteId = activiteId,
                NbQuestionTotal = 0,
                NbQuestionReussi = 0
            };

            // Si l'utilisateur n'a pas d'instance d'exercice
            if (!activiteAvecExercices.Exercices.Any())
            {
                // Si aucun exercice n'existe pour l'activité
                statusActivite.ActiviteStatus = InstanceStatus.AFaire;
                return statusActivite;
            }

            // Trouve tous les status des exercices
            foreach (var exerciceId in activiteAvecExercices.Exercices)
            {
                var statusExercice = await GenerateStatusExerciceAsync(exerciceId, utilisateurId);

                // Ajout du status de l'exercice et nombre de question
                statusActivite.ExercicesStatus.Add(statusExercice);
                statusActivite.NbQuestionReussi += statusExercice.NbQuestionReussi;
                statusActivite.NbQuestionTotal += statusExercice.NbQuestionTotal;
            }

            // Met à jour le status de l'activite
            UpdateActiviteStatus(statusActivite);

            return statusActivite;
        }

        /**
         * Retourne le status de tous les activité de l'utilisateur
         */
        public async Task<List<StatusActivite>> GenerateStatusActivitesAsync(string utilisateurId)
        {
            // TODO seulement selectionné ceux de l'utilisatuer
            var activites = await _context.Activities.ToListAsync();
            var statusActivites = new List<StatusActivite>();

            // Met tous les status d'activité dans une liste
            foreach (var activite in activites)
            {
                var statusActivite = await GenerateStatusActiviteAsync(activite.Id, utilisateurId);
                if (statusActivite != null)
                {
                    statusActivites.Add(statusActivite);
                }
            }

            return statusActivites;
        }

        /**
         * Retourne le status d'un exercice
         */
        public async Task<StatusExercice> GenerateStatusExerciceAsync(int exerciceId, string utilisateurId)
        {
            // Accède au question pour savoir le nombre de question
            var exerciceInstance = await _context.ExercicesInstance
                .Where(e => e.ExerciceId == exerciceId && e.UtilisateurId == utilisateurId)
                .Include(q => q.ReponsesUtilisateur)
                .OrderByDescending(e => e.DateGenere)
                .FirstOrDefaultAsync();

            var nbQuestionTotal = await _context.Questions
                .Where(e => e.ExerciceId == exerciceId)
                .CountAsync();

            // Crée la variable de retour
            var statusExercice = new StatusExercice
            {
                ExerciceId = exerciceId,
                NbQuestionTotal = nbQuestionTotal
            };

            if (exerciceInstance == null)
            {
                statusExercice.ExerciceStatus = InstanceStatus.AFaire;
                return statusExercice;
            }

            // Calcul le nombre de questions réussi
            // Group by pour que les questions avec plusieurs réponses soient seulement comptées une fois
            statusExercice.NbQuestionReussi = exerciceInstance.ReponsesUtilisateur
                .GroupBy(r => r.QuestionId) // Regroupe par QuestionId
                .Select(group => group
                    .OrderByDescending(r => r.DateRepondu) // Trie chaque groupe par DateRepondu pour obtenir la plus récente en premier
                    .FirstOrDefault() // Sélectionne la réponse la plus récente pour chaque QuestionId
                )
                .Where(r => r != null && r.IsCorrect) // Filtre pour ne garder que les réponses correctes
                .Count(); // Compte le nombre de réponses correctes

            // Met à jour le status de l'exerice
            UpdateExerciceStatus(statusExercice);

            return statusExercice;
        }

        /**
         * Modifie le status d'un activité selon les exercices complété
         */
        private void UpdateActiviteStatus(StatusActivite statusActivite)
        {
            if (statusActivite.ExercicesStatus.All(e => e.ExerciceStatus == InstanceStatus.Reussi))
            {
                // Toutes les exercices sont réussi
                statusActivite.ActiviteStatus = InstanceStatus.Reussi;
            }
            else if (statusActivite.ExercicesStatus.All(e => e.ExerciceStatus == InstanceStatus.AFaire))
            {
                // Aucune exercices n'est en cours
                statusActivite.ActiviteStatus = InstanceStatus.AFaire;
            }
            else
            {
                // Au moin une exercice est en cour mais pas tous complété
                statusActivite.ActiviteStatus = InstanceStatus.EnCours;
            }
        }

        /**
         * Modifie le status d'un exercice selon les questions complété
         */
        private void UpdateExerciceStatus(StatusExercice statusExercice)
        {
            if (statusExercice.NbQuestionReussi == statusExercice.NbQuestionTotal)
            {
                // Toutes les questions sont réussi
                statusExercice.ExerciceStatus = InstanceStatus.Reussi;
            }
            else if (statusExercice.NbQuestionReussi == 0)
            {
                // Aucune question répondu
                statusExercice.ExerciceStatus = InstanceStatus.AFaire;
            }
            else
            {
                // Au moins une question est complété
                statusExercice.ExerciceStatus = InstanceStatus.EnCours;
            }
        }

        /// <summary>
        /// Méthode qui s'occupe de retourner le nombre d'exercices commencés, non commencés et réussis dans une activité.
        /// </summary>
        /// <param name="activiteId">ID de l'activité</param>
        /// <returns></returns>
        public async Task<(int nonCommence, int enCours, int termine)?> GetExerciseCountsByStatusAsync(int activiteId, string utilisateurId)
        {
            var statusActivite = await GenerateStatusActiviteAsync(activiteId, utilisateurId);
            if (statusActivite == null) 
            {
                return null;
            };

            int nonCommence = 0;
            int enCours = 0;
            int termine = 0;

            foreach (var statusExercice in statusActivite.ExercicesStatus)
            {
                switch (statusExercice.ExerciceStatus)
                {
                    case InstanceStatus.AFaire:
                        nonCommence++;
                        break;
                    case InstanceStatus.EnCours:
                        enCours++;
                        break;
                    case InstanceStatus.Reussi:
                        termine++;
                        break;
                }
            }

            return (nonCommence, enCours, termine);
        }

        public Task<(int totalNonCommence, int totalEnCours, int totalTermine)?> GetTotalExerciseCountsByStatusAsync()
        {
            throw new NotImplementedException();
        }

        //public async Task<(int totalNonCommence, int totalEnCours, int totalTermine)?> GetTotalExerciseCountsByStatusAsync()
        //{
        //    int totalNonCommence = 0;
        //    int totalEnCours = 0;
        //    int totalTermine = 0;

        //    var allExercices = await _context.ExercicesInstance
        //        .Include(e => e.ReponsesUtilisateur)
        //        .ToListAsync();

        //    foreach (var exercice in allExercices)
        //    {
        //        var statusExercice = await GenerateStatusExerciceAsync(exercice.ExerciceId);

        //        switch (statusExercice.ExerciceStatus)
        //        {
        //            case InstanceStatus.AFaire:
        //                totalNonCommence++;
        //                break;
        //            case InstanceStatus.EnCours:
        //                totalEnCours++;
        //                break;
        //            case InstanceStatus.Reussi:
        //                totalTermine++;
        //                break;
        //        }
        //    }

        //    return (totalNonCommence, totalEnCours, totalTermine);
        //}
    }
}
