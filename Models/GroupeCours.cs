using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models
{
    public class GroupeCours
    {
        [Key]
        public int Id { get; set; } // ID caché pour les utilisateurs

        [Required(ErrorMessage = "Le nom du groupe est obligatoire.")]
        [Display(Name = "Nom du groupe")]
        public string Nom { get; set; } = string.Empty; // Le nom du groupe que les professeurs et les étudiants voient

        [Required(ErrorMessage = "Le lien du groupe est obligatoire.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Le lien doit être une série de 6 chiffres.")]
        [Display(Name = "Lien du groupe")]
        public string Lien { get; set; } = string.Empty; // Lien créé pour rejoindre un groupe cours

        [Required(ErrorMessage = "Le SN est obligatoire.")]
        [Range(1, 3, ErrorMessage = "Le SN doit être entre 1 et 3.")]
        [Display(Name = "SN")]
        public int SN { get; set; } // Chiffre entre 1 et 3 pour identifier le SN

        [Required(ErrorMessage = "Le campus est obligatoire.")]
        [Display(Name = "Campus")]
        public string Campus { get; set; } = string.Empty; // Le campus où le cours est donné

        [Required(ErrorMessage = "Le code du cours est obligatoire.")]
        [Display(Name = "Code du cours")]
        public string Code { get; set; } = string.Empty; // Le code du cours

        [Required(ErrorMessage = "Le numéro du groupe est obligatoire.")]
        [Range(0, int.MaxValue, ErrorMessage = "Le numéro du groupe ne peut pas être négatif.")]
        [Display(Name = "Numéro du groupe")]
        public int Numgroupe { get; set; } // Le numéro du groupe pour le cours

        [Required(ErrorMessage = "L'ID de l'enseignant est obligatoire.")]
        [Display(Name = "ID de l'enseignant")]
        public int EnseignantId { get; set; } // ID du professeur qui gère le groupe

        [Display(Name = "Archivé ?")]
        public Boolean Archiver { get; set; } // Permet de savoir si un groupe est archivé ou non

        // Propriétés de navigation

        [Display(Name = "Enseignant du groupe")]
        public Enseignant? Enseignant { get; set; } // Professeur qui gère le groupe cours

        [Display(Name = "Étudiants du groupe")]
        public List<Etudiant>? Etudiants { get; set; } // Étudiants dans le groupe cours

        [Display(Name = "Liste des activités")]
        public List<Activite>? Activities { get; set; } // Liste des activités dans le groupe cours

        [Display(Name = "Liste des id des étudiants bloquer")]
        public List<int>? EtudiantIdBloquer { get; set; } // Liste des id des etudiants qui sont bloqués 
    }
}
