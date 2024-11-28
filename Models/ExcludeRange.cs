using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class ExcludeRange
    {
        public int Id { get; set; }

        [Required]
        public int Start { get; set; }

        [Required]
        public int End { get; set; }

        [Required]
        public int VariableId { get; set; }

        [JsonIgnore]
        public Variable? Variable { get; set; }
    }
}
