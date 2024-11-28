using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Question
{
    public class QuestionDeroulante: QuestionBase
    {
        [Required(ErrorMessage = "Le texte de la question est requis.")]
        [Display(Name = "Texte de la réponse")]
        public string ReponseTexte { get; set; } = String.Empty;

        public QuestionDeroulante() { }

        public override object Clone()
        {
            QuestionDeroulante clone = (QuestionDeroulante)CloneBase();
            return clone;
        }
    }
}
