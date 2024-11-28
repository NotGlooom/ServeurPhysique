using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models
{
    public enum TimeFrame
    {
        [Display(Name = "Minute")]
        Minute,

        [Display(Name = "Heure")]
        Heure,

        [Display(Name = "Jour")]
        Jour
    }

    public class RappelActivite
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID de l'étudiant est requis.")]
        [Display(Name = "ID de l'étudiant")]
        public int UtilisateurId { get; set; }

        [Required(ErrorMessage = "L'ID de l'activité est requis.")]
        [Display(Name = "ID de l'activité")]
        public int ActiviteId { get; set; }

        [Required(ErrorMessage = "Le temps est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "Le temps doit être supérieur à 0.")]
        [Display(Name = "Temps")]
        public int Time { get; set; }

        [Required(ErrorMessage = "Le cadre de temps est requis.")]
        [EnumDataType(typeof(TimeFrame), ErrorMessage = "Le cadre de temps doit être soit Minute, Heure ou Jour.")]
        [Display(Name = "Cadre de temps")]
        public TimeFrame TimeFrame { get; set; }

        // Propriétés de navigation
        [Display(Name = "Étudiant")]
        public Etudiant? Utilisateur { get; set; }

        [Display(Name = "Activité")]
        public Activite? Activite { get; set; }
    }
}
