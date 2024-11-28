using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Activite
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'activité est obligatoire.")]
        [Display(Name = "Nom de l'activité")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Description de l'activité")]
        public string Description { get; set; } = string.Empty;

        public int NumeroActivite { get; set;  }

        [Display(Name = "Date d'échéance")]
        public DateTime? DateEcheance { get; set; }

        [Display(Name = "Date de publication")]
        public DateTime? DatePublication { get; set; }

        [Display(Name = "Identifiant de l'auteur")]
        public string? AuteurUtilisateurId { get; set; }

        [Display(Name = "Activité publique ?")]
        public bool IsPublique { get; set; } = false;

        public bool IsArchiver { get; set; } = false;

        // Propriétés de navigation
        [Display(Name = "Liste des exercices")]
        public List<Exercice>? Exercices { get; set; }

        [Display(Name = "Liste des rappels d'activité")]
        public List<RappelActivite>? RappelsActivite { get; set; }

        [Display(Name = "Identifiant du groupe de cours")]
        public int? GroupeCoursId { get; set; }

        [JsonIgnore]
        [Display(Name = "Groupe de cours lié")]
        public GroupeCours? GroupeCours { get; set; }

        [Display(Name = "Auteur de l'activite")]
        public Enseignant? Auteur { get; set; }
    }
}
