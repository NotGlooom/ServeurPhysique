namespace ProjetSynthese.Server.Models.DTOs
{
    public class ActiviteDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public int NumeroActivite { get; set; }
        public DateTime? DateEcheance { get; set; }
        public DateTime? DatePublication { get; set; }
        public bool IsPublique { get; set; }
        public bool IsArchiver { get; set; }
        public int GroupeCoursId { get; set; }
        public List<ExerciceDto> Exercices { get; set; } = new();
        public Enseignant? Auteur { get; set; }
    }
}
