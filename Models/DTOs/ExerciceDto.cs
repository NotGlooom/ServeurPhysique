using ProjetSynthese.Server.Models.Question;

namespace ProjetSynthese.Server.Models.DTOs
{
    public class ExerciceDto
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Enonce { get; set; } = string.Empty;
        public int NumeroExercice { get; set; }
        public string? Image { get; set; } = string.Empty;
        public bool DemarcheDisponible { get; set; }
        public bool IsPublique { get; set; }
        public EnseignantDto? Auteur { get; set; }
        public List<VariableDto> Variables { get; set; } = new();
        public List<QuestionBase> Questions { get; set; } = new();
    }
}
