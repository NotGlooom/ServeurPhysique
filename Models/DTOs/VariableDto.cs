using System.ComponentModel.DataAnnotations;

namespace ProjetSynthese.Server.Models.DTOs
{
    public class VariableDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public float Min { get; set; }
        public float Max { get; set; }
        public float Increment { get; set; }
        public string? Unite { get; set; }

        // Liste des plages exclues
        public List<ExcludeRangeDto>? ExcludeRanges { get; set; }
    }
}
