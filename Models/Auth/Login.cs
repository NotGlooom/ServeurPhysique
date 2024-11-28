using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Login
{
    public class Login
    {
        [Required(ErrorMessage = "Le numéro est requis")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
