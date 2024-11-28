namespace ProjetSynthese.Server.Models.DTOs
{
    public class GroupeCoursDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Lien { get; set; } = string.Empty;
        public int SN { get; set; }
        public string Campus { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Numgroupe { get; set; }
        public int EnseignantId { get; set; }
        public bool Archiver { get; set; }

        public List<ActiviteDto> Activities { get; set; } = new();
    }
}
