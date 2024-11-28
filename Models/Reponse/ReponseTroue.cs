using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Reponse
{
    public class ReponseTroue : ReponseBase
    {
        // Position de la réponse par rapport au texte
        [Required(ErrorMessage = "La position dans le texte est requise.")]
        [Display(Name = "Position de la réponse")]
        public int PositionTexte { get; set; }

        public override object Clone()
        {
            ReponseTroue clone = (ReponseTroue)CloneBase();
            clone.PositionTexte = PositionTexte;
            return clone;
        }
    }
}
