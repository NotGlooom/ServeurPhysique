using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models.Question
{
    public class QuestionTroue: QuestionBase
    {
        [Required(ErrorMessage = "Le texte de la question est requis.")]
        [Display(Name = "Texte de la réponse")]
        public string ReponseTexte { get; set; } = String.Empty;

        public QuestionTroue() { }

        public override object Clone()
        {
            QuestionTroue clone = (QuestionTroue)CloneBase();
            clone.ReponseTexte = ReponseTexte;
            return clone;
        }
    }
}
