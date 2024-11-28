using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.Reponse
{
    public class ReponseNumerique : ReponseBase
    {
        // Est-ce que la réponse est calculée
        [Required(ErrorMessage = "Le calcul automatique de la reponse est requis.")]
        [Display(Name = "Calculée automatiquement ?")]
        public bool IsCalculated { get; set; }

        // Marge d'erreur
        [Display(Name = "Marge d'erreur")]
        public float? MargeErreur { get; set; }

        // Chiffre après la virgule
        [Display(Name = "Chiffres après la virgule")]
        public int? ChiffreApresVirgule { get; set; }

        public override object Clone()
        {
            ReponseNumerique clone = (ReponseNumerique)CloneBase();
            clone.IsCalculated = IsCalculated;
            clone.MargeErreur = MargeErreur;
            clone.ChiffreApresVirgule = ChiffreApresVirgule;
            return clone;
        }
    }
}
