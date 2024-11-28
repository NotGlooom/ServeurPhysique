using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Variable
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la variable est obligatoire.")]
        [Display(Name = "Nom de la variable")]
        public string Nom { get; set; } = string.Empty;

        // Valeur minimum de la variable
        [Display(Name = "Valeur minimum")]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "La valeur minimum doit être un nombre valide.")]
        public float Min { get; set; }

        // Valeur maximum de la variable
        [Display(Name = "Valeur maximum")]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "La valeur maximum doit être un nombre valide.")]
        public float Max { get; set; }

        // Increments possible de la variable lorsque généré
        [Display(Name = "Increment")]
        [DefaultValue(0.01f)]
        public float Increment { get; set; }

        [Display(Name = "Unite de mesure")]
        public string? Unite { get; set; }


        [Required(ErrorMessage = "L'ID de l'exercice est requis.")]
        [Display(Name = "ID de l'exercice")]
        public int ExerciceId { get; set; }

        // Liste des plages à exclure
        [Display(Name = "Plages à exclure")]
        public List<ExcludeRange>? ExcludeRanges { get; set; }


        // Propriétés de navigation
        [JsonIgnore]
        [Display(Name = "Exercice associé")]
        public Exercice? Exercice { get; set; }

        [JsonIgnore]
        [Display(Name = "Variable instancier associé")]
        public List<VariableInstance>? VariablesInstance { get; set; }
    }
}
