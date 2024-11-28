using System.Collections.Generic;

namespace ProjetSynthese.Server.Models.DTOs
{
    public class EnseignantDto
    {
        public int Id { get; set; }

        // UtilisateurId as reference to IdentityUser, if needed in the DTO
        public string? UtilisateurId { get; set; } = string.Empty;
        public IdentityUserDto? Utilisateur { get; set; }
        public string? NoEnseignant { get; set; } = string.Empty;

        public string? Nom { get; set; } = string.Empty;

        public string? Prenom { get; set; } = string.Empty;

    }
}
