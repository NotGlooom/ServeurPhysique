using ProjetSynthese.Server.Models.Question;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class ReponseUtilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'id de l'instance d'exercice est requis.")]
        [Display(Name = "Id de l'instance d'exercice répondu")]
        public int ExerciceInstanceId { get; set; }

        [Required(ErrorMessage = "L'id de la question lié est requis.")]
        [Display(Name = "Id de la question")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "La valeur de la réponse est requis.")]
        [Display(Name = "Réponse de l'utilisateur")]
        public string Valeur {  get; set; } = string.Empty;

        [Display(Name = "Position de la réponse")]
        public int? Position { get; set; }

        [Display(Name = "Est-ce que la réponse est exact")]
        public bool IsCorrect { get; set; }

        [Display(Name = "Date de la réponse")]
        public DateTime DateRepondu { get; set; }

        // Propriétés de navigation
        [JsonIgnore]
        [Display(Name = "Exercice instancié associé")]
        public ExerciceInstance? ExerciceInstance { get; set; }

        [JsonIgnore]
        [Display(Name = "Question associé")]
        public QuestionBase? Question { get; set; }
    }
}
