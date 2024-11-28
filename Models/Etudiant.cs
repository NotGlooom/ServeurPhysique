using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetSynthese.Server.Models
{
    public class Etudiant
    {
        [Key]
        public int Id { get; set; }

        public string UtilisateurId { get; set; }  // Clé étrangère vers IdentityUser

        [ForeignKey("UtilisateurId")]
        public IdentityUser utilisateur { get; set; }

        [Required(ErrorMessage = "Le numéro d'étudiant est obligatoire.")]
        [Display(Name = "Numéro d'étudiant")]
        public string numeroEtudiant { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom de l'étudiant est obligatoire.")]
        [Display(Name = "Nom de l'étudiant")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom de l'étudiant est obligatoire.")]
        [Display(Name = "Prénom de l'étudiant")]
        public string Prenom { get; set; } = string.Empty;

        // Propriétés de navigation
        [JsonIgnore]
        [Display(Name = "Cours de l'étudiant")]
        public List<GroupeCours>? Cours { get; set; }

        [JsonIgnore]
        [Display(Name = "Rappels d'activité")]
        public List<RappelActivite>? RappelsActivite { get; set; }

        [JsonIgnore]
        [Display(Name = "Exercices instancié")]
        public List<ExerciceInstance>? ExercicesInstance { get; set; }
    }
}
