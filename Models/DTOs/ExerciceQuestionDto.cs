using ProjetSynthese.Server.Models.Question;

namespace ProjetSynthese.Server.Models.DTOs
{
    public class ExerciceQuestionDto
    {
        public required Exercice Exercice { get; set; }
        public List<QuestionBase>? ListeQuestions { get; set; }
    }
}
