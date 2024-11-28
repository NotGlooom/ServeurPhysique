using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetSynthese.Server.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le message est obligatoire.")]
        [Display(Name = "Message")]
        public string Message { get; set; } = String.Empty;

        [Display(Name = "Identifiant de l'activité associée")]
        public int? ActiviteId { get; set; }

        [Display(Name = "Date de notification")]
        public DateTime? Date { get; set; }

        // Propriétés de navigation
        [Display(Name = "Activité associée")]
        public Activite? Activite { get; set; }
    }
}
