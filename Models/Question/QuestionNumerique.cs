using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models.Question
{
    public class QuestionNumerique: QuestionBase
    {
        [JsonConstructor]
        public QuestionNumerique() { }

        public override object Clone()
        {
            QuestionNumerique clone = (QuestionNumerique)CloneBase();
            return clone;
        }
    }
}
