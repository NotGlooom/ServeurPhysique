using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjetSynthese.Server.Models.Login
{
    public class Registre
    {
        [Required(ErrorMessage = "Le type de compte est requis.")]
        [RegularExpression("^(enseignant|etudiant)$", ErrorMessage = "Le type de compte doit être 'enseignant' ou 'etudiant'.")]
        public string Typecompte { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le nom doit avoir entre 2 et 20 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le prénom doit avoir entre 2 et 20 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le numéro est requis.")]
        [RegularExpression("^(\\d{5}|\\d{7})$", ErrorMessage = "Le numéro doit avoir entre 5 ou 7 caractères.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Le courriel est requis.")]
        [EmailAddress(ErrorMessage = "Le courriel doit être valide.")]
        [CustomValidation(typeof(Registre), nameof(ValiderCourriel), ErrorMessage = "Le courriel doit correspondre au numéro et se terminer par '@cegepoutaouais.qc.ca'.")]
        public string Courriel { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Le mot de passe doit contenir au moins 7 caractères.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&.#])[A-Za-z\\d@$!%*?&.#]{7,}$", ErrorMessage = "Le mot de passe doit contenir au moins une majuscule, un chiffre, et un caractère spécial.")]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Compare("MotDePasse", ErrorMessage = "La confirmation du mot de passe doit correspondre au mot de passe.")]
        public string ConfirmationMotDePasse { get; set; }

        // Méthode de validation personnalisée pour le courriel
        public static ValidationResult ValiderCourriel(string courriel, ValidationContext context)
        {
            var instance = context.ObjectInstance as Registre;
            if (instance == null || string.IsNullOrEmpty(instance.Numero) || string.IsNullOrEmpty(courriel))
            {
                return new ValidationResult("Numéro ou courriel manquant.");
            }

            string courrielAttendu = $"{instance.Numero}@cegepoutaouais.qc.ca";
            if (courriel == courrielAttendu)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Le courriel doit correspondre au numéro et se terminer par '@cegepoutaouais.qc.ca'.");
        }
    }
}
