using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models
{
    public class ExerciceInstance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'exercice li� est requis.")]
        [Display(Name = "Id de l'exercice de base")]
        public int ExerciceId { get; set; }

        [Required(ErrorMessage = "L'utilisateur li� � l'exercice est requis.")]
        [Display(Name = "Id de l'utilisateur qui � l'exerice instanci�")]
        public string UtilisateurId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date g�n�r� est requise.")]
        [Display(Name = "Date � laquel l'exercice � �t� g�n�r�")]
        public DateTime DateGenere { get; set; }

        // Propri�t�s de navigation
        [Display(Name = "Variables instanci� de l'exercice")]
        public List<VariableInstance>? VariablesInstance { get; set; }

        [Display(Name = "R�ponses de l'utilisateur � l'exercice")]
        public List<ReponseUtilisateur>? ReponsesUtilisateur { get; set; }

        [Display(Name = "Exercice")]
        public Exercice? Exercice { get; set; }

        [Display(Name = "Utilisateur qui � l'instance d'exerice")]
        public IdentityUser? Utilisateur { get; set; }
    }

    public enum InstanceStatus
    {
        AFaire,
        EnCours,
        Reussi,
    }
}
