using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Reponse
{
    public class ReponseDeroulante : ReponseBase
    {
        [Required(ErrorMessage = "La position dans le texte est requise.")]
        [Display(Name = "Position de la réponse")]
        public int PositionTexte { get; set; }

        // Est-ce que c'est la bonne réponse
        [Required(ErrorMessage = "La validité de la réponse est requis.")]
        [Display(Name = "Est la bonne réponse ?")]
        public bool IsAnswer { get; set; }

        public override object Clone()
        {
            ReponseDeroulante clone = (ReponseDeroulante)CloneBase();
            clone.PositionTexte = PositionTexte;
            clone.IsAnswer = IsAnswer;
            return clone;
        }
    }
}
