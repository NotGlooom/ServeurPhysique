using ProjetSynthese.Server.Models.Question;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Exercice
    {
        public int Id { get; set; }
        public int? ActiviteId { get; set; }
        public string? AuteurUtilisateurId { get; set; }

        public string Titre { get; set; } = String.Empty;
        public string Enonce { get; set; } = String.Empty;
        public int NumeroExercice { get; set; }
        public string? Image { get; set; }
        public bool DemarcheDisponible { get; set; }
        public bool IsPublique { get; set; }

        // Propriétés de navigation
        [Display(Name = "Auteur de l'exercice")]
        public Enseignant? Auteur { get; set; }

        [Display(Name = "Variables de l'exercice")]
        public List<Variable>? Variables { get; set; }

        [Display(Name = "Questions lié à l'exercice")]
        public List<QuestionBase>? Questions { get; set; }

        [JsonIgnore]
        [Display(Name = "Activité parente")]
        public Activite? Activite { get; set; }

        [JsonIgnore]
        [Display(Name = "Exercices instancier")]
        public List<ExerciceInstance>? ExercicesInstance { get; set; }
    }

}
