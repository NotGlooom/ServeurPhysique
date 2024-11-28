using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetSynthese.Server.Models
{
    public class Enseignant
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UtilisateurId")]
        public string UtilisateurId { get; set; }  // Clé étrangère vers IdentityUser

        [Display(Name = "Numéro de l'enseignant")]
        public string? NoEnseignant { get; set; }

        [Display(Name = "Nom de l'enseignant")]
        public string? Nom { get; set; }

        [Display(Name = "Prénom de l'enseignant")]
        public string? Prenom { get; set; }

        // Propriétés de navigation
        [JsonIgnore]
        [Display(Name = "Groupes de cours")]
        public List<GroupeCours>? GroupeCours { get; set; }

        [JsonIgnore]
        [Display(Name = "Liste des activité que l'enseignant est auteur")]
        public List<Activite>? ListeActiviteAuteur { get; set; }

        [JsonIgnore]
        public IdentityUser? utilisateur { get; set; }
    }
}
