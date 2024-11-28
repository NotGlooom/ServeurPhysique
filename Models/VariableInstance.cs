using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    [PrimaryKey(nameof(ExerciceInstanceId), nameof(VariableId))]
    public class VariableInstance
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'id de l'exercice est requis.")]
        [Display(Name = "Id de l'exercice instancié")]
        public int ExerciceInstanceId { get; set; }

        [Required(ErrorMessage = "L'id de la variable est requis.")]
        [Display(Name = "Id de la variable de base")]
        public int VariableId { get; set; }

        [Required(ErrorMessage = "La valeur de la variable est requis.")]
        [Display(Name = "Valeur de la variable instancié")]
        public float Valeur { get; set; }

        // Propriété de navigation
        [JsonIgnore]
        [Display(Name = "Exercice instancié associé")]
        public ExerciceInstance? ExerciceInstance { get; set; }

        [JsonIgnore]
        [Display(Name = "Variable de base")]
        public Variable? Variable { get; set; }
    }

}
