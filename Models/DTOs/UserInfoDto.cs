namespace ProjetSynthese.Server.Models.DTOs
{
    public class UserInfoDto
    {
        public string Id { get; set; } = "";
        public string Role { get; set; } = ""; // admin, enseignant, etudiant
        public string Nom { get; set; } = "";
        public string Prenom { get; set; } = "";
        public string Courriel { get; set; } = "";
    }
}
