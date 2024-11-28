using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models
{
    public class ExerciceInstance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'exercice lié est requis.")]
        [Display(Name = "Id de l'exercice de base")]
        public int ExerciceId { get; set; }

        [Required(ErrorMessage = "L'utilisateur lié à l'exercice est requis.")]
        [Display(Name = "Id de l'utilisateur qui à l'exerice instancié")]
        public string UtilisateurId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date généré est requise.")]
        [Display(Name = "Date à laquel l'exercice à été généré")]
        public DateTime DateGenere { get; set; }

        // Propriétés de navigation
        [Display(Name = "Variables instancié de l'exercice")]
        public List<VariableInstance>? VariablesInstance { get; set; }

        [Display(Name = "Réponses de l'utilisateur à l'exercice")]
        public List<ReponseUtilisateur>? ReponsesUtilisateur { get; set; }

        [Display(Name = "Exercice")]
        public Exercice? Exercice { get; set; }

        [Display(Name = "Utilisateur qui à l'instance d'exerice")]
        public IdentityUser? Utilisateur { get; set; }
    }

    public enum InstanceStatus
    {
        AFaire,
        EnCours,
        Reussi,
    }
}
