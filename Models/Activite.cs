using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Activite
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'activit� est obligatoire.")]
        [Display(Name = "Nom de l'activit�")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Description de l'activit�")]
        public string Description { get; set; } = string.Empty;

        public int NumeroActivite { get; set;  }

        [Display(Name = "Date d'�ch�ance")]
        public DateTime? DateEcheance { get; set; }

        [Display(Name = "Date de publication")]
        public DateTime? DatePublication { get; set; }

        [Display(Name = "Identifiant de l'auteur")]
        public string? AuteurUtilisateurId { get; set; }

        [Display(Name = "Activit� publique ?")]
        public bool IsPublique { get; set; } = false;

        public bool IsArchiver { get; set; } = false;

        // Propri�t�s de navigation
        [Display(Name = "Liste des exercices")]
        public List<Exercice>? Exercices { get; set; }

        [Display(Name = "Liste des rappels d'activit�")]
        public List<RappelActivite>? RappelsActivite { get; set; }

        [Display(Name = "Identifiant du groupe de cours")]
        public int? GroupeCoursId { get; set; }

        [JsonIgnore]
        [Display(Name = "Groupe de cours li�")]
        public GroupeCours? GroupeCours { get; set; }

        [Display(Name = "Auteur de l'activite")]
        public Enseignant? Auteur { get; set; }
    }
}
