using ProjetSynthese.Server.Models.Question;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Indice
    {
        public int Id { get; set; }
        public string IndiceText { get; set; } = String.Empty;
        public int Essaies { get; set; }

        public int QuestionId { get; set; }

        [JsonIgnore]
        public QuestionBase? Question { get; set; }

        public Indice Clone()
        {
            return new Indice
            {
                IndiceText = this.IndiceText,
                Essaies = this.Essaies,
            };
        }
    }
}
