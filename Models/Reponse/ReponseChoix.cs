using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Reponse
{
    public class ReponseChoix : ReponseBase
    {
        // Position de la réponse dans le checkbox
        [Display(Name = "Position de la réponse")]
        public int? Position { get; set; }

        // Est-ce que c'est la bonne réponse
        [Required(ErrorMessage = "La validité de la réponse est requise.")]
        [Display(Name = "Est la bonne réponse ?")]
        public bool IsAnswer { get; set; }

        public override object Clone()
        {
            ReponseChoix clone = (ReponseChoix)CloneBase();
            clone.Position = Position;
            clone.IsAnswer = IsAnswer;
            return clone;
        }
    }
}
