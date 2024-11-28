using ProjetSynthese.Server.Models.Reponse;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models.Question
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(QuestionChoix), typeDiscriminator: "choix")]
    [JsonDerivedType(typeof(QuestionDeroulante), typeDiscriminator: "deroulante")]
    [JsonDerivedType(typeof(QuestionNumerique), typeDiscriminator: "numerique")]
    [JsonDerivedType(typeof(QuestionTroue), typeDiscriminator: "troue")]
    public abstract class QuestionBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'�nonc� est obligatoire.")]
        [Display(Name = "�nonc� de la question")]
        public string Enonce { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'identifiant de l'exercice est obligatoire.")]
        [Display(Name = "Identifiant de l'exercice")]
        public int ExerciceId { get; set; }

        [Display(Name = "Demander une d�marche ?")]
        public bool DemanderDemarche { get; set; }

        [Display(Name = "Pointage")]
        public int Pointage { get; set; }

        [Display(Name = "Liste des r�ponses")]
        public List<ReponseBase>? Reponses { get; set; }

        [Display(Name = "Liste des Indices")]
        public List<Indice>? Indices { get; set; } // Nullable pour les questions qui n'ont pas d'indices

        // Propri�t�s de navigation
        [JsonIgnore]
        [Display(Name = "Exercice li�")]
        public Exercice? Exercice { get; set; }

        [JsonIgnore]
        [Display(Name = "Réponse de l'utilisateur lié")]
        public List<ReponseUtilisateur>? ReponsesUtilisateur { get; set; }

        public abstract object Clone();

        protected QuestionBase CloneBase()
        {
            var clone = (QuestionBase)MemberwiseClone();

            // Met les id et propriété de navigation au valeur par defaut
            clone.Id = 0;
            ReponsesUtilisateur = null;
            Exercice = null;
            ExerciceId = 0;

            // Clone les reponses et indices
            clone.Reponses = Reponses?.Select(r => (ReponseBase)r.Clone()).ToList();

            clone.Indices = Indices?.Select(i => i.Clone()).ToList();

            return clone;
        }
    }
}
