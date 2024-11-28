using ProjetSynthese.Server.Models.Reponse;

namespace ProjetSynthese.Server.Models.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Enonce { get; set; }
        public bool DemanderDemarche { get; set; }
        public int Pointage { get; set; }
        public List<ReponseBase> Reponses { get; set; } = new();
        public List<IndiceDto> Indices { get; set; } = new();
    }
}
