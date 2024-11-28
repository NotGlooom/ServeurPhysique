using ProjetSynthese.Server.Models.Question;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models.Reponse
{
    [JsonDerivedType(typeof(ReponseChoix), typeDiscriminator: "choix")]
    [JsonDerivedType(typeof(ReponseDeroulante), typeDiscriminator: "deroulante")]
    [JsonDerivedType(typeof(ReponseNumerique), typeDiscriminator: "numerique")]
    [JsonDerivedType(typeof(ReponseTroue), typeDiscriminator: "troue")]
    public abstract class ReponseBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID de la question est requis.")]
        [Display(Name = "ID de la question")]
        public int QuestionId { get; set; }

        // Valeur de la réponse
        [Required(ErrorMessage = "La valeur de la réponse est obligatoire.")]
        [Display(Name = "Valeur de la réponse")]
        public string Valeur { get; set; } = string.Empty;

        // Propriétés de navigation
        [JsonIgnore]
        [Display(Name = "Question associée")]
        public QuestionBase? Question { get; set; }

        public abstract object Clone();

        protected ReponseBase CloneBase()
        {
            var clone = (ReponseBase)this.MemberwiseClone();

            // Met les id et propriété de navigation au valeur par defaut
            clone.Id = 0;
            clone.Question = null;
            clone.QuestionId = 0;

            return clone;
        }
    }
}
